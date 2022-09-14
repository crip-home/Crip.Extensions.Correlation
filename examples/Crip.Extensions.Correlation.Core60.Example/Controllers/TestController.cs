using Crip.Extensions.Correlation.Core60.Example.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Crip.Extensions.Correlation.Core60.Example.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestControllerClient _client;
    private readonly ILogger<TestController> _logger;

    public TestController(ITestControllerClient client, ILogger<TestController> logger)
    {
        _client = client;
        _logger = logger;
    }

    [HttpGet]
    public Dictionary<string, string> Get()
    {
        _logger.LogInformation("Header request");
        return Request.Headers.ToDictionary(
            header => header.Key,
            header => header.Value.ToString());
    }

    [HttpGet("client")]
    public async Task<Dictionary<string, string>> Client()
    {
        _logger.LogInformation("Before http call");

        return await _client.Test();
    }
}