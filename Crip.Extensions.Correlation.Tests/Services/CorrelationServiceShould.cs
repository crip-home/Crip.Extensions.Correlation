using Crip.Extensions.Correlation.Features;
using Crip.Extensions.Correlation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Tests.Services;

public class CorrelationServiceShould
{
    readonly Mock<IOptions<CorrelationIdOptions>> _options = new();

    [Fact, Trait("Category", "Unit")]
    public void Constructor_CanCreateInstance()
    {
        var act = () => new CorrelationService(_options.Object);

        act.Should().NotThrow();
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfOptionsNotProvided()
    {
        var act = () => new CorrelationService(null!);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'options')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Set_AddsCorrelationFeatureToContext()
    {
        var correlation = Correlation();
        var context = new DefaultHttpContext();

        correlation.Set(context, "id");

        context.Features.Get<ICorrelationFeature>()
            .Should().NotBeNull()
            .And.BeEquivalentTo(new CorrelationFeature("id"));
    }

    [Fact, Trait("Category", "Unit")]
    public void Set_FailsIfHttpContextNotProvided()
    {
        var correlation = Correlation();

        var act = () => correlation.Set(null!, "id");

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'httpContext')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_FailsIfHttpContextNotProvided()
    {
        var correlation = Correlation();

        var act = () => correlation.Get(null!);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'httpContext')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_NullIfFeatureNotAvailable()
    {
        var correlation = Correlation();
        var context = new DefaultHttpContext();

        var result = correlation.Get(context);

        result.Should().BeNull();
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_ReturnsCorrelationFeatureIdentifier()
    {
        var correlation = Correlation();
        var context = new DefaultHttpContext();
        context.Features.Set<ICorrelationFeature>(new CorrelationFeature("id"));

        var result = correlation.Get(context);

        result.Should().Be("id");
    }

    [Fact, Trait("Category", "Unit")]
    public void Scope_ReturnsDefaultKeyAndFeatureValue()
    {
        var correlation = Correlation();
        var context = new DefaultHttpContext();
        context.Features.Set<ICorrelationFeature>(new CorrelationFeature("id"));
        MockOptions(new());

        var result = correlation.Scope(context);

        result.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { CorrelationIdOptions.CorrelationPropertyName, "id" }
        });
    }

    [Fact, Trait("Category", "Unit")]
    public void Scope_ReturnsCustomizePropertyName()
    {
        var correlation = Correlation();
        var context = new DefaultHttpContext();
        context.Features.Set<ICorrelationFeature>(new CorrelationFeature("id"));
        MockOptions(new() { PropertyName = "Custom" });

        var result = correlation.Scope(context);

        result.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            { "Custom", "id" }
        });
    }

    private void MockOptions(CorrelationIdOptions correlationIdOptions) =>
        _options.Setup(options => options.Value).Returns(correlationIdOptions);

    private CorrelationService Correlation() => new(_options.Object);
}