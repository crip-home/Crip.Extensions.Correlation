using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crip.Extensions.Correlation.Core31.Example.Clients;

public interface ITestControllerClient
{
    Task<Dictionary<string, string>> Test();
}