using AStar.FilesApi.Config;
using AStar.FilesApi.Files;
using AStar.FilesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class ListDuplicatesShould : IClassFixture<ListDuplicatesFixture>
{
    private const int ExpectedResultCountWithDefaultSearchParameters = 10;
    private readonly ListDuplicatesFixture mockFilesFixture;

    public ListDuplicatesShould(ListDuplicatesFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

    [Fact]
    public void ReturnBadRequestWhenNoSearchFolderSpecified()
    {
        var response = mockFilesFixture.SUT.Handle(new(){ SearchFolder = string.Empty }).Result as BadRequestObjectResult;

        _ = response?.Value.Should().Be("A Search folder must be specified.");
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsTopLevelFolderOnlyWhichIsEmpty()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", Recursive=false}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<DuplicateGroup>)response!.Value!;

        _ = value.Count.Should().Be(0);
    }

    [Fact]
    public void GetTheExpectedListOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<DuplicateGroup>)response!.Value!;

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public Task GetTheExpectedFirstPageOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 20, CurrentPage = 1}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<DuplicateGroup>)response!.Value!;

        value.Count.Should().Be(20);
        return Verify(value);
    }

    [Fact]
    public Task GetTheExpectedSecondPageOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 20, CurrentPage = 2}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<DuplicateGroup>)response!.Value!;

        value.Count.Should().Be(14);
        return Verify(value);
    }
}
