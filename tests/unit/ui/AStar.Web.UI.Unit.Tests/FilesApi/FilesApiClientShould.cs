using AStar.Web.UI.MockMessageHandlers;
using AStar.Web.UI.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.FilesApi;

public class FilesApiClientShould
{
    private readonly string irrelevantUrl = "https://doesnot.matter.com";
    private readonly ILogger<FilesApiClient> dummyLogger = NullLogger<FilesApiClient>.Instance;

    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApIsiUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Could not get a response from the Files API.");
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetHealthAsyncWhenCheckFails()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Health Check failed.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetHealthAsyncWhenCheckSucceeds()
    {
        var handler = new MockHealthCheckSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("OK");
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheCountEndpoint()
    {
        var handler = new MockSuccessMessageWithValue0HttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetFilesCountAsync(new SearchParameters());

        response.Should().Be(0);
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheCountDuplicatesEndpoint()
    {
        var handler = new MockSuccessMessageWithValue0HttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetDuplicateFilesCountAsync(new SearchParameters());

        response.Should().Be(0);
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheListEndpoint()
    {
        var handler = new MockSuccessMessageWithFileListHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetFilesAsync(new SearchParameters());

        response.Count().Should().Be(2);
    }

    [Fact]
    public async Task ReturnExpectedResponseFromTheListDuplicatesEndpoint()
    {
        var handler = new MockSuccessMessageWithFileListDuplicatesHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.GetDuplicateFilesAsync(new SearchParameters());

        response.Count.Should().Be(3);
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForSoftDeletionWhenSuccessful()
    {
        var handler = new MockHealthCheckSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.MarkForSoftDeletionAsync("not relevant");

        response.Should().Be("Marked for soft deletion");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForSoftDeletionWhenFailure()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.MarkForSoftDeletionAsync("not relevant");

        response.Should().Be("Delete failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForSoftDeletionWhenFailure()
    {
        var handler = new MockInternalServerErrorHttpMessageHandlerForUnMarkForDeletion();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.UndoMarkForSoftDeletionAsync("not relevant");

        response.Should().Be("Undo mark for deletion failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForSoftDeletionWhenSuccessful()
    {
        var handler = new MockDeletionSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.UndoMarkForSoftDeletionAsync("not relevant");

        response.Should().Be("Mark for soft deletion has been undone");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForHardDeletionWhenSuccessful()
    {
        var handler = new MockDeletionSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.MarkForHardDeletionAsync("not relevant");

        response.Should().Be("Marked for hard deletion.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForHardDeletionWhenFailure()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.MarkForHardDeletionAsync("not relevant");

        response.Should().Be("Delete failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForHardDeletionWhenSuccessful()
    {
        var handler = new MockHealthCheckSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.UndoMarkForHardDeletionAsync("not relevant");

        response.Should().Be("Mark for hard deletion has been undone");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForHardDeletionWhenFailure()
    {
        var handler = new MockInternalServerErrorHttpMessageHandlerForUnMarkForDeletion();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.UndoMarkForHardDeletionAsync("not relevant");

        response.Should().Be("Undo mark for deletion failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForMovingWhenSuccessful()
    {
        var handler = new MockDeletionSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.MarkForMovingAsync("not relevant");

        response.Should().Be("Mark for moving was successful");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromMarkForMovingWhenFailure()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.MarkForMovingAsync("not relevant");

        response.Should().Be("Delete failed...");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForMovingWhenSuccessful()
    {
        var handler = new MockHealthCheckSuccessHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.UndoMarkForMovingAsync("not relevant");

        response.Should().Be("Undo mark for moving was successful");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromUndoMarkForMovingWhenFailure()
    {
        var handler = new MockInternalServerErrorHttpMessageHandlerForUnMarkForDeletion();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new(irrelevantUrl)
        };

        var sut = new FilesApiClient(httpClient, dummyLogger);

        var response = await sut.UndoMarkForMovingAsync("not relevant");

        response.Should().Be("Undo mark for deletion failed...");
    }
}
