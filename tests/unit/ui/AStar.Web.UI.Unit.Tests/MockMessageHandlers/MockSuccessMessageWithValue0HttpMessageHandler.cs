using System.Net;
using System.Text.Json;
using AStar.Web.UI.Shared;

namespace AStar.Web.UI.MockMessageHandlers;

public class MockSuccessMessageWithValue0HttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("0"),
        });
}
