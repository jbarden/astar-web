using AStar.FilesApi.Config;
using AStar.FilesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class DuplicatesCountShould : IClassFixture<DuplicatesCountFixture>
{
    private readonly DuplicatesCountFixture mockFilesFixture;

    public DuplicatesCountShould(DuplicatesCountFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

    [Fact]
    public void ReturnBadRequestWhenNoSearchFolderSpecified()
    {
        var response = mockFilesFixture.SUT.Handle(new(){ SearchFolder = string.Empty }).Result as BadRequestObjectResult;

        _ = response?.Value.Should().Be("A Search folder must be specified.");
    }

    [Fact]
    public void ReturnBadRequestForImageCount()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Images}).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Only Duplicate counts are supported by this endpoint, please call the non-duplicate-specific endpoint.");
    }

    [Fact]
    public void ReturnBadRequestForAllFilesCount()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.All}).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Only Duplicate counts are supported by this endpoint, please call the non-duplicate-specific endpoint.");
    }

    [Fact]
    public void GetTheExpectedCountOfDuplicateFileGroupsWhenStartingAtTheRootFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Duplicates}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(34);
    }

    [Fact]
    public void GetTheExpectedCountOfDuplicateFileGroupsWhenStartingAtSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true, SearchType = SearchType.Duplicates}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(16);
    }
}
