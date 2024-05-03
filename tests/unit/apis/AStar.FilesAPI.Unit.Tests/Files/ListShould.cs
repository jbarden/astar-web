using System.Text.Json;
using AStar.FilesApi.Config;
using AStar.FilesApi.Models;
using AStar.FilesAPI.Helpers;
using AStar.Web.Domain;
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

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.Count.Should().Be(0);
    }

    [Fact]
    public void GetTheExpectedListOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public void GetTheFullListOfFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 10_000}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.Count.Should().Be(mockFilesFixture.MockFilesContext.Files.Count());
    }

    [Fact]
    public void GetTheFullListContainingTheExpectedFilesWhenTheFilterAppliedCapturesAllFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.All, ItemsPerPage = 10_000}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "_Host.cshtml", FullName = "c:\\temp\\Blazor.Bootstrap\\AStar.Web\\AStar.Web.UI\\Pages\\_Host.cshtml", Size = 2228L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "z20080523 003.jpg", FullName = "c:\\temp\\Home\\month03\\z20080523 003.jpg", Size = 211127L, Height = 1234L, Width = 5678L });
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllImageFiles()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"c:\", SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "_Host.cshtml", FullName = "c:\\temp\\Blazor.Bootstrap\\AStar.Web\\AStar.Web.UI\\Pages\\_Host.cshtml", Size = 2228L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = ".gitignore", FullName = "c:\\temp\\Famous\\.gitignore", Size = 6352L });
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.Count.Should().Be(ExpectedResultCountWithDefaultSearchParameters);
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatTargetsSpecificFolderRecursively()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous", Recursive = true}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = ".editorconfig", FullName = "c:\\temp\\Famous\\.editorconfig", Size = 12756L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-1pmqr3.jpg", FullName = "c:\\temp\\Famous\\Sara Jean Underwood\\Playmate\\lingerie\\sideboob\\ass\\wallhaven-1pmqr3.jpg", Size = 2688298L });
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.Count.Should().Be(4);
    }

    [Fact]
    public void GetTheExpectedFilesWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var response = mockFilesFixture.SUT.Handle(new(){SearchFolder = @"C:\Temp\Famous\coats", Recursive = false, SearchType = SearchType.Images}).Result as OkObjectResult;

        var value = ((IReadOnlyCollection<FileInfoDto>)response!.Value!);

        _ = value.First().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-3k313y.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-3k313y.jpg", Size = 387073L });
        _ = value.Last().Should().BeEquivalentTo(new FileInfoDto() { Name = "wallhaven-yxre17.jpg", FullName = "c:\\temp\\Famous\\coats\\wallhaven-yxre17.jpg", Size = 179564L });
    }
}
