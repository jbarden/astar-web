using System.Text;
using System.Text.Json;
using AStar.FilesApi.Config;
using AStar.FilesApi.Controllers;
using AStar.FilesApi.Files;
using AStar.FilesAPI.Unit.Tests.Helpers;
using AStar.Web.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.FilesAPI.Unit.Tests.Files;

public class FilesCounterControllerShould
{
    [Fact]
    public async Task ReturnZeroWhenNoCriteriaSpecified()
    {
        var mockFilesContext = new MockFilesContext().CreateContext();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new())).Result as OkObjectResult;

        _ = response!.Value.Should().Be(0);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatCapturesAllFiles()
    {
        var mockFilesContext = new MockFilesContext().CreateContext();

        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"c:\", SearchType = SearchType.All})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(mockFilesContext.Files.Count());
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatTargetsTopLevelFolderOnlyWhichIsEmpty()
    {
        var mockFilesContext = new MockFilesContext().CreateContext();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"d:\", RecursiveSubDirectories = false})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(0);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var mockFilesContext = new MockFilesContext().CreateContext();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"C:\Temp\Famous", RecursiveSubDirectories = true})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(95);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var mockFilesContext = new MockFilesContext().CreateContext();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"C:\Temp\Famous\coats", RecursiveSubDirectories = false, SearchType = SearchType.Images})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(4);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderRecursively()
    {
        var mockFilesContext = new MockFilesContext().CreateContext();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"C:\Temp\Famous", RecursiveSubDirectories = true, SearchType = SearchType.Images})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(95);
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
    }
}
