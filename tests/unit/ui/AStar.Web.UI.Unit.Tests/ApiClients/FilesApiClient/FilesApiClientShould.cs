using AStar.FilesApi.Client.SDK.Models;
using AStar.Web.UI.Helpers;
using AStar.Web.UI.MockMessageHandlers;
using AStar.Web.UI.Shared;

namespace AStar.Web.UI.ApiClients.FilesApiClient;

public class FilesApiClientShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApIsiUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Could not get a response from the Files API.");
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetHealthAsyncWhenCheckFails()
    {
        var sut = ApiClientFactory<AStar.FilesApi.Client.SDK.FilesApi.FilesApiClient>.CreateInternalServerErrorClient("Health Check failed.");

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Health Check failed.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetHealthAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("OK");
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheCountEndpoint()
    {
        var handler = new MockSuccessHttpMessageHandler("Count");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.GetFilesCountAsync(new SearchParameters());

        response.Should().Be(0);
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheCountDuplicatesEndpoint()
    {
        const int MockDuplicatesCountValue = 1234;
        var handler = new MockSuccessHttpMessageHandler("CountDuplicates") {Counter = MockDuplicatesCountValue};
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.GetDuplicateFilesCountAsync(new SearchParameters());

        response.Should().Be(MockDuplicatesCountValue);
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheListEndpoint()
    {
        var handler = new MockSuccessHttpMessageHandler("ListFiles");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.GetFilesAsync(new SearchParameters());

        response.Count().Should().Be(2);
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheListDuplicatesEndpoint()
    {
        var handler = new MockSuccessHttpMessageHandler("ListDuplicates");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.GetDuplicateFilesAsync(new SearchParameters());

        response.Count.Should().Be(3);
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForSoftDeletionWhenSuccessful()
    {
        var handler = new MockSuccessHttpMessageHandler("");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.MarkForSoftDeletionAsync(Guid.NewGuid());

        response.Should().Be("Marked for soft deletion");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForSoftDeletionWhenFailure()
    {
        var sut = FilesApiClientFactory.CreateInternalServerErrorClient("Delete failed...");

        var response = await sut.MarkForSoftDeletionAsync(Guid.NewGuid());

        response.Should().Be("Delete failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForSoftDeletionWhenFailure()
    {
        var sut = FilesApiClientFactory.CreateInternalServerErrorClient("Undo mark for deletion failed...");

        var response = await sut.UndoMarkForSoftDeletionAsync(Guid.NewGuid());

        response.Should().Be("Undo mark for deletion failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForSoftDeletionWhenSuccessful()
    {
        var handler = new MockDeletionSuccessHttpMessageHandler();
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.UndoMarkForSoftDeletionAsync(Guid.NewGuid());

        response.Should().Be("Mark for soft deletion has been undone");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForHardDeletionWhenSuccessful()
    {
        var handler = new MockDeletionSuccessHttpMessageHandler();
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.MarkForHardDeletionAsync(Guid.NewGuid());

        response.Should().Be("Marked for hard deletion.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForHardDeletionWhenFailure()
    {
        var sut = FilesApiClientFactory.CreateInternalServerErrorClient("Delete failed...");

        var response = await sut.MarkForHardDeletionAsync(Guid.NewGuid());

        response.Should().Be("Delete failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForHardDeletionWhenSuccessful()
    {
        var handler = new MockSuccessHttpMessageHandler("");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.UndoMarkForHardDeletionAsync(Guid.NewGuid());

        response.Should().Be("Mark for hard deletion has been undone");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForHardDeletionWhenFailure()
    {
        var sut = FilesApiClientFactory.CreateInternalServerErrorClient("Undo mark for deletion failed...");

        var response = await sut.UndoMarkForHardDeletionAsync(Guid.NewGuid());

        response.Should().Be("Undo mark for deletion failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForMovingWhenSuccessful()
    {
        var handler = new MockDeletionSuccessHttpMessageHandler();
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.MarkForMovingAsync(Guid.NewGuid());

        response.Should().Be("Mark for moving was successful");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForMovingWhenFailure()
    {
        var sut = FilesApiClientFactory.CreateInternalServerErrorClient("Delete failed...");

        var response = await sut.MarkForMovingAsync(Guid.NewGuid());

        response.Should().Be("Delete failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForMovingWhenSuccessful()
    {
        var handler = new MockSuccessHttpMessageHandler("");
        var sut = FilesApiClientFactory.Create(handler);

        var response = await sut.UndoMarkForMovingAsync(Guid.NewGuid());

        response.Should().Be("Undo mark for moving was successful");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForMovingWhenFailure()
    {
        var sut = FilesApiClientFactory.CreateInternalServerErrorClient("Undo mark for deletion failed...");

        var response = await sut.UndoMarkForMovingAsync(Guid.NewGuid());

        response.Should().Be("Undo mark for deletion failed...");
    }
}
