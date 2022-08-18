namespace Crip.Extensions.Correlation.Features;

/// <summary>
/// Request correlation feature contract.
/// </summary>
public interface ICorrelationFeature
{
    /// <summary>
    /// Gets correlation identifier.
    /// </summary>
    string Id { get; }
}