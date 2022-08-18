using Crip.Extensions.Correlation.Features;

namespace Crip.Extensions.Correlation.Tests.Features;

public class CorrelationFeatureShould
{
    [Fact, Trait("Category", "Unit")]
    public void Constructor_SetsIdValue()
    {
        var feature = new CorrelationFeature("id");

        feature.Id.Should().Be("id");
    }
}