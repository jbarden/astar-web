using System.Text.Json;
using AStar.FilesApi.Config;
using AStar.FilesAPI.Helpers;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class CountShould : IClassFixture<CountFixture>
{
    private readonly CountFixture mockFilesFixture;

    public CountShould(CountFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

    [Fact]
    public void ReturnBadRequestWhenNoSearchFolderSpecified()
    {
        var response = mockFilesFixture.SUT.Handle(new(){ SearchFolder = string.Empty }).Result as BadRequestObjectResult;

        _ = response?.Value.Should().Be("A Search folder must be specified.");
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(mockFilesFixture.MockFilesContext.Files.Count());
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(288);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsTopLevelFolderOnlyWhichIsEmpty()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"d:\", Recursive = false}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(0);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(95);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(4);
    }

    [Fact]
    public void ReturnBadRequestForDuplicates()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Duplicates}).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Duplicate searches are not supported by this endpoint, please call the duplicate-specific endpoint.");
    }
}
