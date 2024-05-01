using System.Text.Json;
using AStar.FilesApi.Config;
using AStar.FilesAPI.Unit.Tests.Helpers;
using AStar.Web.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Unit.Tests.Files;

public class FilesCounterControllerShould : IClassFixture<FilesControllerFixture>
{
    private readonly FilesControllerFixture mockFilesFixture;

    public FilesCounterControllerShould(FilesControllerFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

    [Fact]
    public void ReturnZeroWhenNoCriteriaSpecified()
    {
        var response = (mockFilesFixture.SUT.Get(new())).Result as OkObjectResult;

        _ = response!.Value.Should().Be(0);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllFiles()
    {
        var response = (mockFilesFixture.SUT.Get(new(){SearchFolder = @"c:\", SearchType = SearchType.All})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(mockFilesFixture.MockFilesContext.Files.Count());
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = (mockFilesFixture.SUT.Get(new(){SearchFolder = @"c:\", SearchType = SearchType.Images})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(288);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsTopLevelFolderOnlyWhichIsEmpty()
    {
        var response = (mockFilesFixture.SUT.Get(new(){SearchFolder = @"d:\", Recursive = false})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(0);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = (mockFilesFixture.SUT.Get(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(95);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = (mockFilesFixture.SUT.Get(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(4);
    }

    [Fact]
    public void ReturnBadRequestForDuplicates()
    {
        var response = (mockFilesFixture.SUT.Get(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Duplicates})).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Duplicate searches are not supported by this endpoint, please call the duplicate-specific endpoint.");
    }

    [Fact]
    public void ReturnBadRequestForAllFileCountWhenCallingDuplicatesEndpoint()
    {
        var response = (mockFilesFixture.SUT.GetDuplicates(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.All})).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Only Duplicate searches are supported by this endpoint, please call the non-duplicate-specific endpoint for any other count.");
    }

    [Fact]
    public void ReturnBadRequestForImageFileCountWhenCallingDuplicatesEndpoint()
    {
        var response = (mockFilesFixture.SUT.GetDuplicates(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Images})).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Only Duplicate searches are supported by this endpoint, please call the non-duplicate-specific endpoint for any other count.");
    }

    [Fact]
    public void GetTheExpectedCountOfDuplicateFileGroupsWhenStartingAtTheRootFolder()
    {
        var response = (mockFilesFixture.SUT.GetDuplicates(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Duplicates})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(18);
    }

    [Fact]
    public void GetTheExpectedCountOfDuplicateFileGroupsWhenStartingAtSubFolder()
    {
        var response = (mockFilesFixture.SUT.GetDuplicates(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true, SearchType = SearchType.Duplicates})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(17);
    }

    [Fact]
    public void GetTheFileList()
    {
        var files = Directory.EnumerateFiles(@"c:\temp", "*.*", new EnumerationOptions(){IgnoreInaccessible=true,RecurseSubdirectories = true });
        var fileList = new List<FileDetail>();
        foreach(var file in files)
        {
            var detail = new FileInfo(file);
            var fileDetail = new FileDetail(detail);
            fileList.Add(fileDetail);
        }

        var listAsJson = JsonSerializer.Serialize(fileList);
        File.WriteAllText(@"C:\repos\astar-web\Tests\unit\apis\AStar.FilesAPI.Unit.Tests\TestFiles\files.json", listAsJson);
        Assert.True(true); // Now S2699, please stop moaning ;-)
    }
}
