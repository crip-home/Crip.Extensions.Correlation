using System.Text.Json;

namespace Crip.Extensions.Correlation.Core60.Example.Clients;

public class TestControllerClient : ITestControllerClient
{
    private readonly HttpClient _client;
    private readonly ILogger<TestControllerClient> _logger;

    public TestControllerClient(HttpClient client, ILogger<TestControllerClient> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<Dictionary<string, string>> Test()
    {
        _logger.LogInformation("Before actual request");
        var response = await _client.GetAsync("/test");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Dictionary<string, string>>(content)!;
    }
}