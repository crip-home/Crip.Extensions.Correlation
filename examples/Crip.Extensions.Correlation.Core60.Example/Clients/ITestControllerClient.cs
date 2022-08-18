namespace Crip.Extensions.Correlation.Core60.Example.Clients;

public interface ITestControllerClient
{
    Task<Dictionary<string, string>> Test();
}