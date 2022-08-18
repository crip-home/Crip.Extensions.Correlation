using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;

namespace Crip.Extensions.Correlation;

/// <summary>
/// Application builder extensions.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Registers <see cref="CorrelationIdMiddleware"/> and <see cref="CorrelationIdLoggingMiddleware"/>
    /// middlewares in to application pipeline.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
    /// <returns>A reference to the <paramref name="app"/> after the operation has completed.</returns>
    public static IApplicationBuilder UseCorrelation(this IApplicationBuilder app) => app
        .UseMiddleware<CorrelationIdMiddleware>()
        .UseMiddleware<CorrelationIdLoggingMiddleware>();
}