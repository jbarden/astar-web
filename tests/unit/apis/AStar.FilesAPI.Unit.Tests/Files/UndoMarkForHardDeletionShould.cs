using AStar.FilesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class UndoMarkForHardDeletionShould : IClassFixture<UndoMarkForHardDeletionFixture>
{
    private readonly UndoMarkForHardDeletionFixture mockFilesFixture;

    public UndoMarkForHardDeletionShould(UndoMarkForHardDeletionFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

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
    public void GetTheExpectedCountWhenUndoMarkFileForDeletionWasSuccessful()
    {
        var testFile = mockFilesFixture.MockFilesContext.Files.First();
        testFile.HardDeletePending = true;
        mockFilesFixture.MockFilesContext.SaveChanges();

        _ = mockFilesFixture.SUT.Handle(Path.Combine(testFile.DirectoryName, testFile.FileName)) as OkObjectResult;

        mockFilesFixture.MockFilesContext.Files.Count(file => file.DirectoryName == testFile.DirectoryName && file.FileName == testFile.FileName && file.HardDeletePending).Should().Be(0);
    }
}
