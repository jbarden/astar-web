using AStar.Web.Ui.ApiClients;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.Unit.Tests.ApiClients;

public class FilesApiClientShould
{
    [Fact]
    public async Task ReturnFailedFromGetHealthAsyncApiUnreachable()
    {
        var httpClient = new HttpClient();
        var sut = new FilesApiClient(httpClient, NullLogger<FilesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Could not get a response from the Files API.");
    }
}
