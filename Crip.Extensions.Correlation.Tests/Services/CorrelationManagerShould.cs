using Crip.Extensions.Correlation.Services;
using Microsoft.Extensions.Options;

namespace Crip.Extensions.Correlation.Tests.Services;

public class CorrelationManagerShould
{
    private readonly Mock<IOptions<CorrelationIdOptions>> _options = new();
    private readonly Mock<ILogger<CorrelationManager>> _logger = new();

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfOptionsNotProvided()
    {
        var act = () => new CorrelationManager(null!, _logger.Object);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'options')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfDelegateNotProvided()
    {
        var act = () => new CorrelationManager(_options.Object, null!);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'logger')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_CanGetValueAfterSet()
    {
        var manager = Manager();

        manager.Set("test");

        manager.Get().Should().Be("test");
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_ChildThreadDoesNotAffectValue()
    {
        var manager = Manager();

        manager.Set("test");

        RunInThread(() =>
        {
            manager.Set("test-1");
            manager.Get().Should().Be("test-1");
        });

        manager.Get().Should().Be("test");
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_ChildThreadGetsSameValueAsParent()
    {
        var manager = Manager();

        manager.Set("test");

        RunInThread(() => manager.Get().Should().Be("test"));

        manager.Get().Should().Be("test");
    }

    [Fact, Trait("Category", "Unit")]
    public void Scope_CreatesNewCorrelationValue()
    {
        var manager = Manager();
        MockOptionsPropertyName("key");

        var scope = manager.Scope();

        scope.ContainsKey("key").Should().BeTrue();
        scope["key"]!.ToString().Should().NotBeNullOrEmpty();
    }

    [Fact, Trait("Category", "Unit")]
    public void Scope_UsesExistingScopeValue()
    {
        var manager = Manager();
        manager.Set("test");
        MockOptionsPropertyName("key");

        var scope = manager.Scope();

        scope.ContainsKey("key").Should().BeTrue();
        scope["key"]!.ToString().Should().Be("test");
    }

    [Fact, Trait("Category", "Unit")]
    public async Task Use_CreatesLoggingScopeWithProvidedCorrelationId()
    {
        var manager = Manager();
        MockOptionsPropertyName("key");
        await manager.Use("test", async () => await Task.CompletedTask);

        _logger.Verify(
            logger => logger.BeginScope(It.Is<Dictionary<string, object?>>(scope => scope["key"]!.ToString() == "test")),
            Times.Once);
    }

    [Fact, Trait("Category", "Unit")]
    public async Task Use_CreatesLoggingScopeWithNewCorrelationId()
    {
        var manager = Manager();
        MockOptionsPropertyName("key");
        await manager.Use(async () => await Task.CompletedTask);

        _logger.Verify(
            logger => logger.BeginScope(It.Is<Dictionary<string, object?>>(scope => !string.IsNullOrWhiteSpace(scope["key"]!.ToString()))),
            Times.Once);
    }

    [Fact, Trait("Category", "Unit")]
    public void Use_SynchronouslyCreatesLoggingScopeWithProvidedCorrelationId()
    {
        var manager = Manager();
        MockOptionsPropertyName("key");
        manager.Use("test", VoidAction);

        _logger.Verify(
            logger => logger.BeginScope(It.Is<Dictionary<string, object?>>(scope => scope["key"]!.ToString() == "test")),
            Times.Once);
    }

    [Fact, Trait("Category", "Unit")]
    public void Use_SynchronouslyCreatesLoggingScopeWithNewCorrelationId()
    {
        var manager = Manager();
        MockOptionsPropertyName("key");
        manager.Use(VoidAction);

        _logger.Verify(
            logger => logger.BeginScope(It.Is<Dictionary<string, object?>>(scope => !string.IsNullOrWhiteSpace(scope["key"]!.ToString()))),
            Times.Once);
    }

    private void MockOptionsPropertyName(string key) =>
        _options
            .SetupGet(options => options.Value)
            .Returns(new CorrelationIdOptions { PropertyName = key });

    private CorrelationManager Manager() => new(_options.Object, _logger.Object);

    private static void RunInThread(Action action) => Task.Run(async () =>
    {
        action();
        await Task.CompletedTask;
    }).GetAwaiter().GetResult();

    private static void VoidAction() { }
}