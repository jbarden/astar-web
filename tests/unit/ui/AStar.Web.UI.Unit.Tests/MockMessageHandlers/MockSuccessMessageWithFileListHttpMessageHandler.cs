using System.Net;
using System.Text.Json;
using AStar.Web.UI.Models;

namespace AStar.Web.UI.MockMessageHandlers;

public class MockSuccessMessageWithFileListHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new List<FileInfoDto>() { new(), new() }))
        });
}
