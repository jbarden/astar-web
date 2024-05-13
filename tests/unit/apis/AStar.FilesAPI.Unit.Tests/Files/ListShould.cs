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
        const int FilesNotSoftDeletedOrPendingDeletionCount = 341;
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 10_000}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(FilesNotSoftDeletedOrPendingDeletionCount);
    }

    [Fact]
    public void GetTheFullListContainingTheExpectedFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 10_000}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "Mothers Day 2010.psd", FullName = "c:\\temp\\M Day 2010\\Mothers Day 2010.psd", Size = 119880324L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "read.lock", FullName = "c:\\temp\\Blazor.Bootstrap\\AStar.Web\\AStar.Web.UI\\.vs\\AStar.Web.UI\\FileContentIndex\\read.lock", Size = 0L });
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "Mothers Day 2010 copy.jpg", FullName = "c:\\temp\\M Day 2010\\Mothers Day 2010 copy.jpg", Size = 10435815L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-l87ygp.jpg", FullName = "c:\\temp\\Famous\\Jes Macallan\\wallhaven-l87ygp.jpg", Size = 5510486L });
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-rrz291.png", FullName = "c:\\temp\\Famous\\Rebecca Ferguson\\wallhaven-rrz291.png", Size = 9099482L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-2y5139.jpg", FullName = "c:\\temp\\Famous\\actress\\Sara Jean Underwood - Copy - Copy\\Playmate\\wallhaven-2y5139.jpg", Size = 4922103L });
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        _ = value.Count.Should().Be(3);
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-851jjy.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-851jjy.jpg", Size = 296929L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-yxre17.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-yxre17.jpg", Size = 179564L });
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheSizeDescendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\", Recursive = true, SearchType = SearchType.Images, SortOrder = SortOrder.SizeDescending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "Mothers Day 2010 copy.jpg", FullName = "c:\\temp\\M Day 2010\\Mothers Day 2010 copy.jpg", Size = 10435815L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-l87ygp.jpg", FullName = "c:\\temp\\Famous\\Jes Macallan\\wallhaven-l87ygp.jpg", Size = 5510486L });
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheSizeAscendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\", Recursive = true, SearchType = SearchType.Images, SortOrder = SortOrder.SizeAscending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "favicon.png", FullName = "c:\\temp\\Blazor.Bootstrap\\AStar.Web\\AStar.Web.UI\\wwwroot\\favicon.png", Size = 7074L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "6.jpg", FullName = "c:\\temp\\1st Year Frame\\6.jpg", Size = 162205L });
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheNameDescendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images, SortOrder = SortOrder.NameDescending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-yxre17.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-yxre17.jpg", Size = 179564L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-851jjy.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-851jjy.jpg", Size = 296929L });
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolderAnHonourTheNameAscendingSortOrder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images, SortOrder = SortOrder.NameAscending}).Result as OkObjectResult;

        var value = (IReadOnlyCollection<FileInfoDto>)response!.Value!;

        using var scope = new AssertionScope();
        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-851jjy.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-851jjy.jpg", Size = 296929L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-yxre17.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-yxre17.jpg", Size = 179564L });
    }
}
