using AStar.FilesAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesAPI.Files;

public class UndoMarkForMovingShould : IClassFixture<UndoMarkForMovingFixture>
{
    private readonly UndoMarkForMovingFixture mockFilesFixture;

    public UndoMarkForMovingShould(UndoMarkForMovingFixture mockFilesFixture) => this.mockFilesFixture = mockFilesFixture;

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
    public void GetTheExpectedCountWhenMarkFileForMovingWasSuccessful()
    {
        var testFile = mockFilesFixture.MockFilesContext.Files.First();
        testFile.NeedsToMove = true;
        mockFilesFixture.MockFilesContext.SaveChanges();

        _ = mockFilesFixture.SUT.Handle(Path.Combine(testFile.DirectoryName, testFile.FileName)) as OkObjectResult;

        mockFilesFixture.MockFilesContext.Files.Count(file => file.DirectoryName == testFile.DirectoryName && file.FileName == testFile.FileName && file.NeedsToMove).Should().Be(0);
    }
}
