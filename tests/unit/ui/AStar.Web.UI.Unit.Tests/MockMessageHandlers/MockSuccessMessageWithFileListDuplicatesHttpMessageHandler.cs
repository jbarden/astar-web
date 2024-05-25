using System.Net;
using System.Text.Json;
using AStar.Web.UI.Models;
using AStar.Web.UI.Pages;

namespace AStar.Web.UI.MockMessageHandlers;

public class MockSuccessMessageWithFileListDuplicatesHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new List<DuplicateGroup>() { new(), new(), new() }))
        });
}
