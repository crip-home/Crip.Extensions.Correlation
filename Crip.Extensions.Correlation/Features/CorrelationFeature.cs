namespace Crip.Extensions.Correlation.Features;

/// <summary>
/// Request correlation feature.
/// </summary>
public class CorrelationFeature : ICorrelationFeature
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationFeature"/> class.
    /// </summary>
    /// <param name="id">Correlation identifier.</param>
    public CorrelationFeature(string id)
    {
        Id = id;
    }

    /// <inheritdoc />
    public string Id { get; }
}