using Crip.Extensions.Correlation.Services;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Tests.Handlers;

public class CorrelationIdHeaderHandlerShould : DelegatingHandlerTestBase
{
    private readonly Mock<IOptions<CorrelationIdOptions>> _options = new();
    private readonly Mock<IHttpCorrelationAccessor> _correlation = new();

    [Fact, Trait("Category", "Unit")]
    public void Constructor_DoesNotFail()
    {
        var act = () => new CorrelationIdHeaderHandler(_options.Object);

        act.Should().NotThrow();
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfOptionsNotProvided()
    {
        var act = () => new CorrelationIdHeaderHandler(null!);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'options')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfCorrelationNotProvided()
    {
        var act = () => new CorrelationIdHeaderHandler(_options.Object);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'correlation')");
    }

    [Fact, Trait("Category", "Unit")]
    public async Task SendAsync_SetsHeaderValue()
    {
        var handler = Handler();
        var request = new HttpRequestMessage();
        MockOptions(new() { Header = "key" });
        MockCorrelationGet("header-id");

        await Invoke(handler, request);

        request.Headers
            .Any(header => header.Key == "key" && header.Value.FirstOrDefault() == "header-id")
            .Should().BeTrue();
    }

    [Fact, Trait("Category", "Unit")]
    public async Task SendAsync_SetsRandomHeaderValue()
    {
        var handler = Handler();
        var request = new HttpRequestMessage();
        MockOptions(new() { Header = "key" });
        MockCorrelationGet(null);

        await Invoke(handler, request);

        request.Headers
            .Any(header => header.Key == "key" && string.IsNullOrWhiteSpace(header.Value.FirstOrDefault()) == false)
            .Should().BeTrue();
    }

    [Fact, Trait("Category", "Unit")]
    public async Task SendAsync_FailsIfNoRequestProvided()
    {
        var handler = Handler();
        Func<Task> act = async () => await Invoke(handler, null!);

        await act.Should()
            .ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'request')");
    }

    [Fact, Trait("Category", "Unit")]
    public async Task SendAsync_DosNotAddHeaderIfAlreadyPresented()
    {
        var handler = Handler();
        var request = new HttpRequestMessage();
        request.Headers.Add("key", "existing");
        MockOptions(new() { Header = "key" });
        MockCorrelationGet("header-id");

        await Invoke(handler, request);

        request.Headers.Any(header => header.Value.FirstOrDefault() == "existing").Should().BeTrue();
        request.Headers.Any(header => header.Value.FirstOrDefault() == "header-id").Should().BeFalse();
    }

    private CorrelationIdHeaderHandler Handler() => new(_options.Object) { InnerHandler = InnerHandler };

    private void MockOptions(CorrelationIdOptions correlationIdOptions) =>
        _options.Setup(options => options.Value).Returns(correlationIdOptions);

    private void MockCorrelationGet(string? result) =>
        _correlation
            .Setup(correlation => correlation.Get())
            .Returns(result);
}