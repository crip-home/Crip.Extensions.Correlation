using System;
using Microsoft.AspNetCore.Http;

namespace Crip.Extensions.Correlation.Services;

/// <summary>
/// HTTP request correlation identifier accessor.
/// </summary>
public class HttpCorrelationAccessor : IHttpCorrelationAccessor
{
    private readonly ICorrelationService _correlation;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpCorrelationAccessor"/> class.
    /// </summary>
    /// <param name="correlation">The correlation identifier service.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="correlation"/> or <paramref name="httpContextAccessor"/>
    /// is not provided.
    /// </exception>
    public HttpCorrelationAccessor(
        ICorrelationService correlation,
        IHttpContextAccessor httpContextAccessor)
    {
        _correlation = correlation ?? throw new ArgumentNullException(nameof(correlation));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc />
    public string? Get() =>
        _correlation.Get(_httpContextAccessor.HttpContext);
}