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
    private readonly ICorrelationManager _manager;
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="logger">The logging service.</param>
    /// <param name="manager">The correlation identifier manager.</param>
    /// <param name="next">The request delegate.</param>
    /// <exception cref="System.ArgumentNullException">
    /// If <paramref name="logger"/> or <paramref name="manager"/> or <paramref name="next"/>
    /// is not provided.
    /// </exception>
    public CorrelationIdLoggingMiddleware(
        ILogger<CorrelationIdLoggingMiddleware> logger,
        ICorrelationManager manager,
        RequestDelegate next)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _manager = manager ?? throw new ArgumentNullException(nameof(manager));
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

        var scope = _manager.Scope();

        using (_logger.BeginScope(scope)) await _next(context);
    }
}