using AStar.Web.UI.ImagesApi;
using AStar.Web.UI.Unit.Tests.MockMessageHandlers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.Unit.Tests.ApiClients;

public class ImagesApiClientShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApIsiUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();

#pragma warning disable S1075 // URIs should not be hardcoded
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://doesnot.matter.com")
        };
#pragma warning restore S1075 // URIs should not be hardcoded

        var sut = new ImagesApiClient(httpClient, NullLogger<ImagesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Could not get a response from the Images API.");
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetHealthAsyncWhenCheckFails()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler();

#pragma warning disable S1075 // URIs should not be hardcoded
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://doesnot.matter.com")
        };
#pragma warning restore S1075 // URIs should not be hardcoded

        var sut = new ImagesApiClient(httpClient, NullLogger<ImagesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Health Check failed.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetHealthAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler();

#pragma warning disable S1075 // URIs should not be hardcoded
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://doesnot.matter.com")
        };
#pragma warning restore S1075 // URIs should not be hardcoded

        var sut = new ImagesApiClient(httpClient, NullLogger<ImagesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("OK");
    }
}
