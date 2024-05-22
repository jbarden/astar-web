using AStar.FilesApi.Config;
using AStar.FilesApi.Models;
using AStar.FilesAPI.Helpers;
using AStar.Web.Domain;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class ListShould : IClassFixture<ListFixture>
{
    private const int ExpectedResultCountWithDefaultSearchParameters = 10;
    private readonly ListFixture mockFilesFixture;

    public ListShould(ListFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

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

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(0);
    }

    [Fact]
    public void GetTheExpectedListOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public void GetTheFullListOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        const int FilesNotSoftDeletedOrPendingDeletionCount = 364;
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 10_000}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public Task GetTheFullListContainingTheExpectedFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 10_000}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(4);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheSizeDescendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\", Recursive = true, SearchType = SearchType.Images, SortOrder = SortOrder.SizeDescending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheSizeAscendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\", Recursive = true, SearchType = SearchType.Images, SortOrder = SortOrder.SizeAscending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheNameDescendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images, SortOrder = SortOrder.NameDescending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }

    [Fact]
    public Task GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheNameAscendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images, SortOrder = SortOrder.NameAscending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        return Verify(value);
    }
}
