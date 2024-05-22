using AStar.FilesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class MarkForSoftDeletionShould : IClassFixture<MarkForSoftDeletionFixture>
{
    private readonly MarkForSoftDeletionFixture mockFilesFixture;

    public MarkForSoftDeletionShould(MarkForSoftDeletionFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

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
    public void GetTheExpectedCountWhenMarkFileForDeletionWasSuccessful()
    {
        var testFile = mockFilesFixture.MockFilesContext.Files.First();

        _ = mockFilesFixture.SUT.Handle(Path.Combine(testFile.DirectoryName, testFile.FileName)) as OkObjectResult;

        mockFilesFixture.MockFilesContext.Files.Count(file => file.DirectoryName == testFile.DirectoryName && file.FileName == testFile.FileName && file.SoftDeletePending).Should().Be(1);
    }
}
