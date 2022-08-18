using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Crip.Extensions.Correlation.Services;

/// <summary>
/// Request correlation service contract.
/// </summary>
public interface ICorrelationService
{
    /// <summary>
    /// Try get correlation identifier from a HTTP request.
    /// </summary>
    /// <param name="httpContext">The HTTP request context.</param>
    /// <returns>Correlation identifier value if available.</returns>
    string? Get(HttpContext httpContext);

    /// <summary>
    /// Set correlation id to HTTP request context.
    /// </summary>
    /// <param name="httpContext">HTTP request context.</param>
    /// <param name="id">correlation identifier.</param>
    void Set(HttpContext httpContext, string id);

    /// <summary>
    /// Create correlation logging scope from HTTP context.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <returns>Correlation identifier logging scope.</returns>
    Dictionary<string, object?> Scope(HttpContext httpContext);
}