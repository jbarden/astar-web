using AStar.FilesApi.Controllers;
using AStar.FilesApi.Files;
using AStar.FilesAPI.Unit.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.FilesAPI.Unit.Tests.Files;

public class FilesCounterControllerShould
{
    [Fact]
    public async Task GetTheExpectedCountWhenNoFiltersApplied()
    {
        var mockFilesContext =await MockFilesContext.CreateAsync();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new())).Result as OkObjectResult;

        _ = response!.Value.Should().Be(12);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatCapturesAllKnownFiles()
    {
        var mockFilesContext =await MockFilesContext.CreateAsync();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"c:\"})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(12);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatTargetsTopLevelFolderOnlyWhichIsEmpty()
    {
        var mockFilesContext =await MockFilesContext.CreateAsync();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"c:\", RecursiveSubDirectories = false})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(0);
    }

    [Fact]
    public async Task GetTheExpectedCountWhenFilterAppliedThatTargetsTopLevelFolderRecursively()
    {
        var mockFilesContext =await MockFilesContext.CreateAsync();
        var sut = new FilesCounterController(mockFilesContext, NullLogger<FilesControllerBase>.Instance);

        var response = (await sut.Get(new(){SearchFolder = @"c:\", RecursiveSubDirectories = true})).Result as OkObjectResult;

        _ = response!.Value.Should().Be(12);
    }
}
