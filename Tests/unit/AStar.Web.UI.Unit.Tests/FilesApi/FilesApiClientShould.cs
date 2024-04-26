using AStar.Web.UI.FilesApi;
using AStar.Web.UI.Unit.Tests.MockMessageHandlers;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.Unit.Tests.FilesApi;

public class FilesApiClientShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApIsiUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://doesnot.matter.com")
        };

        var sut = new FilesApiClient(httpClient, NullLogger<FilesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Could not get a response from the Files API.");
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetHealthAsyncWhenCheckFails()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://doesnot.matter.com")
        };

        var sut = new FilesApiClient(httpClient, NullLogger<FilesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Health Check failed.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetHealthAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://doesnot.matter.com")
        };

        var sut = new FilesApiClient(httpClient, NullLogger<FilesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("OK");
    }
}
