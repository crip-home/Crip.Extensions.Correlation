using System;
using System.Threading.Tasks;
using Crip.Extensions.Correlation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Crip.Extensions.Correlation;

/// <summary>
/// Request correlation identifier logging middleware.
/// </summary>
public class CorrelationIdLoggingMiddleware
{
    private readonly ILogger<CorrelationIdLoggingMiddleware> _logger;
    private readonly ICorrelationService _correlation;
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logging service.</param>
    /// <param name="correlation">The correlation identifier service.</param>
    /// <param name="next">The request delegate.</param>
    /// <exception cref="System.ArgumentNullException">
    /// If <paramref name="logger"/> or <paramref name="correlation"/> or <paramref name="next"/>
    /// is not provided.
    /// </exception>
    public CorrelationIdLoggingMiddleware(
        ILogger<CorrelationIdLoggingMiddleware> logger,
        ICorrelationService correlation,
        RequestDelegate next)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _correlation = correlation ?? throw new ArgumentNullException(nameof(correlation));
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    /// <summary>
    /// Invokes action for the specified context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>Next middleware output.</returns>
    /// <exception cref="System.ArgumentNullException">
    /// If <paramref name="context"/> not provided.
    /// </exception>
    public async Task Invoke(HttpContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        var scope = _correlation.Scope(context);

        using (_logger.BeginScope(scope)) await _next(context);
    }
}