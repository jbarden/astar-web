using AStar.Web.UI.ApiClients.FilesApi;
using AStar.Web.UI.MockMessageHandlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.Helpers;

internal static class FilesApiClientFactory
{
    private static readonly ILogger<FilesApiClient> DummyLogger = NullLogger<FilesApiClient>.Instance;
    private static readonly string IrrelevantUrl = "https://doesnot.matter.com";

    public static FilesApiClient Create(HttpMessageHandler mockHttpMessageHandler)
    {
        var httpClient = new HttpClient(mockHttpMessageHandler)
        {
            BaseAddress = new(IrrelevantUrl)
        };

        return new FilesApiClient(httpClient, DummyLogger);
    }

    public static FilesApiClient CreateInternalServerErrorClient(string errorMessage)
    {
        var handler = new MockInternalServerErrorHttpMessageHandler(errorMessage);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(IrrelevantUrl)
        };

        return new FilesApiClient(httpClient, DummyLogger);
    }
}
