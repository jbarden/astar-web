using System.Net;

namespace AStar.Web.UI.MockMessageHandlers;

public class MockInternalServerErrorHttpMessageHandlerForMarkForMoving : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("[Undo ]Mark for moving failed...") });
}
