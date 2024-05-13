using System.IO;
using AStar.FilesApi.Config;
using AStar.FilesAPI.Helpers;
using AStar.Web.Domain.Types;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class MarkForDeletionShould : IClassFixture<MarkForDeletionFixture>
{
    private readonly MarkForDeletionFixture mockFilesFixture;

    public MarkForDeletionShould(MarkForDeletionFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("Ssdfsdfsdfsdfdsarch")]
    public void ReturnBadRequestWhenNoValidPathSpecified(string fileWithPath)
    {
        var response = mockFilesFixture.SUT.Handle(fileWithPath) as BadRequestObjectResult;

        _ = response?.Value.Should().Be("A valid file with path must be specified.");
    }

    [Fact]
    public void GetTheExpectedCountWhenFilterAppliedThatCapturesAllSupportedImageTypesFromStartingSubFolder()
    {
        var testFile = mockFilesFixture.MockFilesContext.Files.First();

        var response = mockFilesFixture.SUT.Handle(Path.Combine(testFile.DirectoryName, testFile.FileName)) as OkObjectResult;

        mockFilesFixture.MockFilesContext.Files.Count(file => file.DirectoryName == testFile.DirectoryName && file.FileName == testFile.FileName && file.DeletePending).Should().Be(1);
    }
}
