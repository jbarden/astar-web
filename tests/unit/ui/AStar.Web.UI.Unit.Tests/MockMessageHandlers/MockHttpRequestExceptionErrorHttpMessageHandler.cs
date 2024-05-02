namespace AStar.Web.UI.MockMessageHandlers;

public class MockHttpRequestExceptionErrorHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => throw new HttpRequestException();
}
