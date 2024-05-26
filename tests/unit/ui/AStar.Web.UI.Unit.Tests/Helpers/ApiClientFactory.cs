using AStar.Web.UI.ApiClients;
using AStar.Web.UI.MockMessageHandlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.Helpers;

internal static class ApiClientFactory<T> where T : IApiClient
{
    private static readonly ILogger<T> DummyLogger = NullLogger<T>.Instance;
    private static string IrrelevantUrl => "https://doesnot.matter.com";

    public static T Create(HttpMessageHandler mockHttpMessageHandler)
    {
        var httpClient = new HttpClient(mockHttpMessageHandler)
        {
            BaseAddress = new(IrrelevantUrl)
        };

        return (T)Activator.CreateInstance(typeof(T), httpClient, DummyLogger)!;
    }

    public static T CreateInternalServerErrorClient(string errorMessage)
    {
        var handler = new MockInternalServerErrorHttpMessageHandler(errorMessage);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(IrrelevantUrl)
        };

        return (T)Activator.CreateInstance(typeof(T), httpClient, DummyLogger)!;
    }
}
