using System;
using System.Linq;
using System.Threading.Tasks;
using Crip.Extensions.Correlation.Exceptions;
using Crip.Extensions.Correlation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation;

/// <summary>
/// Request correlation identifier middleware. Sets context identifier from
/// request headers or generates new value for it.
/// </summary>
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptionsMonitor<CorrelationIdOptions> _options;
    private readonly ICorrelationService _correlation;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware delegate.</param>
    /// <param name="options">The correlation identifier options.</param>
    /// <param name="correlation">The correlation service.</param>
    /// <exception cref="System.ArgumentNullException">
    /// If <paramref name="next"/> or <paramref name="options"/> is not provided.
    /// </exception>
    public CorrelationIdMiddleware(
        RequestDelegate next,
        IOptionsMonitor<CorrelationIdOptions> options,
        ICorrelationService correlation)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _correlation = correlation ?? throw new ArgumentNullException(nameof(correlation));
    }

    private CorrelationIdOptions Options => _options.CurrentValue;

    /// <summary>
    /// Invokes middleware with the specified context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Next middleware output.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// If <paramref name="context"/> is not provided.
    /// </exception>
    public Task Invoke(HttpContext context)
    {
        if (context.Request?.Headers is null) throw new RequestHeadersMissingException();

        _correlation.Set(context, GetIdentifier(context.Request) ?? CreateIdentifier());

        if (Options.IncludeInResponse)
        {
            // Apply the correlation identifier to the response header for client side tracking.
            context.Response.OnStarting(AddToResponseHeaders(context));
        }

        return _next(context);
    }

    /// <summary>
    /// Create new unique identifier.
    /// </summary>
    /// <returns>New and unique correlation identifier.</returns>
    protected virtual string CreateIdentifier() => Guid.NewGuid().ToString();

    private Func<Task> AddToResponseHeaders(HttpContext context) => () =>
    {
        var correlationId = _correlation.Get(context);
        var headers = context.Response.Headers;
        if (headers?.ContainsKey(Options.Header) is false)
        {
            headers.Add(Options.Header, correlationId);
        }

        return Task.CompletedTask;
    };

    private string? GetIdentifier(HttpRequest request)
    {
        if (request.Headers.TryGetValue(Options.Header, out var correlationIds))
        {
            return correlationIds.FirstOrDefault();
        }

        if (request.Cookies.TryGetValue(Options.Cookie, out var correlationId))
        {
            return correlationId;
        }

        return null;
    }
}