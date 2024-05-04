using AStar.Web.UI.MockMessageHandlers;
using AStar.Web.UI.Shared;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.FilesApi;

public class FilesApiClientShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApIsiUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new("https://doesnot.matter.com")
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
            BaseAddress = new("https://doesnot.matter.com")
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
            BaseAddress = new("https://doesnot.matter.com")
        };

        var sut = new FilesApiClient(httpClient, NullLogger<FilesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("OK");
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheCountEndpoint()
    {
        var handler = new MockSuccessMessageWithValue0HttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new("https://doesnot.matter.com")
        };

        var sut = new FilesApiClient(httpClient, NullLogger<FilesApiClient>.Instance);

        var response = await sut.GetFilesCountAsync(new SearchParameters());

        response.Should().Be(0);
    }
}
