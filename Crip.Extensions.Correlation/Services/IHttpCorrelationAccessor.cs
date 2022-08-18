namespace Crip.Extensions.Correlation.Services;

/// <summary>
/// HTTP request correlation identifier accessor contract.
/// </summary>
public interface IHttpCorrelationAccessor
{
    /// <summary>
    /// Get correlation identifier from HTTP context accessor.
    /// </summary>
    /// <returns>Correlation identifier if available in HTTP context.</returns>
    string? Get();
}