using System;
using System.Collections.Generic;
using Crip.Extensions.Correlation.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Services;

/// <summary>
/// Request correlation service.
/// </summary>
public class CorrelationService : ICorrelationService
{
    private readonly IOptions<CorrelationIdOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationService"/> class.
    /// </summary>
    /// <param name="options">The correlation identifier options.</param>
    public CorrelationService(IOptions<CorrelationIdOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public string? Get(HttpContext httpContext)
    {
        if (httpContext is null) throw new ArgumentNullException(nameof(httpContext));

        return httpContext.Features.Get<ICorrelationFeature>()?.Id;
    }

    /// <inheritdoc />
    public void Set(HttpContext httpContext, string id)
    {
        if (httpContext is null) throw new ArgumentNullException(nameof(httpContext));

        CorrelationFeature feature = new(id);
        httpContext.Features.Set<ICorrelationFeature>(feature);
    }

    /// <inheritdoc />
    public Dictionary<string, object?> Scope(HttpContext httpContext) =>
        new() { { _options.Value.PropertyName, Get(httpContext) } };
}