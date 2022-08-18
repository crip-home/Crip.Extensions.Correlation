using System.Globalization;
using Crip.Extensions.Correlation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Crip.Extensions.Correlation.Tests.Middlewares;

public class CorrelationIdLoggingMiddlewareShould
{
    readonly Mock<ILogger<CorrelationIdLoggingMiddleware>> _logger = new();
    readonly Mock<ICorrelationService> _correlation = new();
    readonly RequestDelegate _delegate = _ => Task.CompletedTask;

    [Fact, Trait("Category", "Unit")]
    public void Constructor_CanCreateInstance()
    {
        var act = () => new CorrelationIdLoggingMiddleware(_logger.Object, _correlation.Object, _delegate);

        act.Should().NotThrow();
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfLoggerNotProvided()
    {
        var act = () => new CorrelationIdLoggingMiddleware(null!, _correlation.Object, _delegate);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'logger')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfCorrelationNotProvided()
    {
        var act = () => new CorrelationIdLoggingMiddleware(_logger.Object, null!, _delegate);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'correlation')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfDelegateNotProvided()
    {
        var act = () => new CorrelationIdLoggingMiddleware(_logger.Object, _correlation.Object, null!);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'next')");
    }

    [Fact, Trait("Category", "Unit")]
    public async Task Invoke_LogsDelegateActionWithRequiredScopeProperty()
    {
        var logger = CreateLoggerFactory().CreateLogger<CorrelationIdLoggingMiddleware>();

        Task RequestDelegate(HttpContext ctx)
        {
            logger.LogDebug("context log message");
            return Task.CompletedTask;
        }

        CorrelationIdLoggingMiddleware middleware = new(logger, _correlation.Object, RequestDelegate);
        MockCorrelationScope("Key", "Value");

        using var _ = TestCorrelator.CreateContext();
        logger.LogInformation("Before");
        await middleware.Invoke(new DefaultHttpContext());
        logger.LogInformation("After");

        var logs = TestCorrelator.GetLogEventsFromCurrentContext().Select(FormatLogEvent).ToList();
        const string sourceContext = "SourceContext: \"Crip.Extensions.Correlation.CorrelationIdLoggingMiddleware\"";
        logs.Should().BeEquivalentTo(
            $"Information: Before {{ {sourceContext} }}",
            $"Debug: context log message {{ {sourceContext}, Key: \"Value\" }}",
            $"Information: After {{ {sourceContext} }}");
    }

    [Fact, Trait("Category", "Unit")]
    public async Task Invoke_FailsIfContextNotProvided()
    {
        CorrelationIdLoggingMiddleware middleware = new(_logger.Object, _correlation.Object, _delegate);

        Func<Task> act = async () => await middleware.Invoke(null!);

        await act.Should()
            .ThrowExactlyAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    private void MockCorrelationScope(string key, string value) =>
        _correlation
            .Setup(correlation => correlation.Scope(It.IsAny<HttpContext>()))
            .Returns(new Dictionary<string, object?> { { key, value } });

    private static ILoggerFactory CreateLoggerFactory() =>
        new SerilogLoggerFactory(CreateCorrelatorLogger());

    private static Logger CreateCorrelatorLogger() =>
        new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestCorrelator()
            .Enrich.FromLogContext()
            .CreateLogger();

    private static string FormatLogEvent(LogEvent logEvent)
    {
        const string template = "{Level}: {Message:lj} {Properties}";

        var culture = CultureInfo.InvariantCulture;
        MessageTemplateTextFormatter formatter = new(template, culture);
        StringWriter writer = new();
        formatter.Format(logEvent, writer);

        return writer.ToString();
    }
}