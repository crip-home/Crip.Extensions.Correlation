using System.Net;

namespace Crip.Extensions.Correlation.Tests.Handlers;

public class DelegatingHandlerTestBase
{
    protected static DelegatingHandler InnerHandler => new TestHandler();

    protected static async Task<HttpResponseMessage> Invoke(DelegatingHandler handler, HttpRequestMessage request)
    {
        var invoker = new HttpMessageInvoker(handler);
        return await invoker.SendAsync(request, new CancellationToken());
    }

    private class TestHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK), cancellationToken);
    }
}