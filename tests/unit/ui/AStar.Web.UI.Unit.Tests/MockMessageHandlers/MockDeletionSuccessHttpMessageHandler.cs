using System.Net;

namespace AStar.Web.UI.MockMessageHandlers;

public class MockDeletionSuccessHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Marked for deletion.")
        });
}
