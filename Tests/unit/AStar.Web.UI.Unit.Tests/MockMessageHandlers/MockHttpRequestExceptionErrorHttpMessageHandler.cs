namespace AStar.Web.UI.Unit.Tests.MockMessageHandlers;

public class MockHttpRequestExceptionErrorHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => throw new HttpRequestException();
}
