using System.Text.Json;

namespace Crip.Extensions.Correlation.Core60.Example.Clients;

public class TestControllerClient : ITestControllerClient
{
    private readonly HttpClient _client;

    public TestControllerClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Dictionary<string, string>> Test()
    {
        var response = await _client.GetAsync("/test");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Dictionary<string, string>>(content)!;
    }
}