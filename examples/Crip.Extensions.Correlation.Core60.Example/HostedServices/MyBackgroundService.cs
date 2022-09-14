using Crip.Extensions.Correlation.Core60.Example.Clients;
using Crip.Extensions.Correlation.Services;

namespace Crip.Extensions.Correlation.Core60.Example.HostedServices;

public class MyBackgroundService : BackgroundService
{
    private readonly ILogger<MyBackgroundService> _logger;
    private readonly ICorrelationManager _correlation;
    private readonly ITestControllerClient _client;

    public MyBackgroundService(
        ILogger<MyBackgroundService> logger,
        ICorrelationManager correlation,
        ITestControllerClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _correlation = correlation ?? throw new ArgumentNullException(nameof(correlation));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Background task before use");

        await _correlation.Use("some-test-correlation-001", async () =>
        {
            _logger.LogDebug("Background task after set");

            var result = await _client.Test();

            _logger.LogDebug("response: {@Result}", result);
        });

        _logger.LogDebug("Background task after use");

    }
}