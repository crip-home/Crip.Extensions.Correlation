using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crip.Extensions.Correlation.Core50.Example.Clients;

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
        return JsonSerializer.Deserialize<Dictionary<string, string>>(content);
    }
}