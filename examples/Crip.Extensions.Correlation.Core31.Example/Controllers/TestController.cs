using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crip.Extensions.Correlation.Core31.Example.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Crip.Extensions.Correlation.Core31.Example.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestControllerClient _client;

    public TestController(ITestControllerClient client)
    {
        _client = client;
    }

    [HttpGet]
    public Dictionary<string, string> Get() =>
        Request.Headers.ToDictionary(
            header => header.Key,
            header => header.Value.ToString());

    [HttpGet("client")]
    public async Task<Dictionary<string, string>> Client() =>
        await _client.Test();
}