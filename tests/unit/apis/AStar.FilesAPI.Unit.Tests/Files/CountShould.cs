using AStar.FilesApi.Config;
using AStar.FilesAPI.Helpers;
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
        const int FilesNotSoftDeletedOrPendingDeletionCount = 341;

        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllImageFiles()
    {
        const int FilesNotSoftDeletedOrPendingDeletionCount = 248;

        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
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
        const int FilesNotSoftDeletedOrPendingDeletionCount = 81;
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        const int FilesNotSoftDeletedOrPendingDeletionCount = 3;

        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public void ReturnBadRequestForDuplicates()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\", Recursive = true, SearchType = SearchType.Duplicates}).Result as BadRequestObjectResult;

        _ = response!.Value.Should().Be("Duplicate searches are not supported by this endpoint, please call the duplicate-specific endpoint.");
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursivelyButIncludeSoftDeleted()
    {
        const int FilesNotSoftDeletedOrPendingDeletionCount = 89;

        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true, IncludeSoftDeleted = true}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursivelyButIncludeMarkedForDeletion()
    {
        const int FilesNotSoftDeletedOrPendingDeletionCount = 87;

        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true, IncludeMarkedForDeletion = true}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursivelyButIncludeSoftDeletedAndIncludeMarkedForDeletion()
    {
        const int FilesNotSoftDeletedOrPendingDeletionCount = 95;

        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true, IncludeSoftDeleted = true, IncludeMarkedForDeletion = true}).Result as OkObjectResult;

        _ = response!.Value.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }
}
