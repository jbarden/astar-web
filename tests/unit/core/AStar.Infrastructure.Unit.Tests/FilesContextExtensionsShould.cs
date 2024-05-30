using AStar.Infrastructure.Data;
using AStar.Infrastructure.Fixtures;

namespace AStar.Infrastructure;

public class FilesContextExtensionsShould(FilesContextFixture filesContextFixture) : IClassFixture<FilesContextFixture>
{
    private readonly FilesContext sut = filesContextFixture.SUT;

    [Fact]
    public Task ReturnFilteredFilesFromTheRootFolderOnlyWhenRecursiveIsFalse()
    {
        var response = sut.Files.FilterBySearchFolder("c:\\temp", false, CancellationToken.None);

        response.Count().Should().Be(10);

        return Verify(response);
    }

    [Fact]
    public Task ReturnFilteredFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrue()
    {
        var response = sut.Files.FilterBySearchFolder("c:\\temp", true, CancellationToken.None);

        response.Count().Should().Be(34);

        return Verify(response);
    }

    [Fact]
    public Task ReturnFilteredFilesFromTheSubFolderAndSubDirectoriesWhenRecursiveIsTrue()
    {
        var response = sut.Files.FilterBySearchFolder("c:\\temp\\1st Year Frame", true, CancellationToken.None);

        response.Count().Should().Be(22);

        return Verify(response);
    }

    [Fact]
    public Task ReturnFilteredFilesFromTheSubFolderOnlyWhenRecursiveIsFalse()
    {
        var response = sut.Files.FilterBySearchFolder("c:\\temp\\1st Year Frame", false, CancellationToken.None);

        response.Count().Should().Be(17);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedAndDeletePendingAreTrue()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "searchTypeNotRelevant", true, true, CancellationToken.None);

        response.Count().Should().Be(34);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedIsTrueButDeletePendingIsFalse()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "searchTypeNotRelevant", true, false, CancellationToken.None);

        response.Count().Should().Be(28);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedIsFalseButDeletePendingIsTrue()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "searchTypeNotRelevant", false, true, CancellationToken.None);

        response.Count().Should().Be(26);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedAndDeletePendingAreFalse()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "searchTypeNotRelevant", false, false, CancellationToken.None);

        response.Count().Should().Be(23);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedAndDeletePendingAreTrue_ImagesOnly()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "Images", true, true, CancellationToken.None);

        response.Count().Should().Be(22);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedIsTrueButDeletePendingIsFalse_ImagesOnly()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "Images", true, false, CancellationToken.None);

        response.Count().Should().Be(18);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedIsFalseButDeletePendingIsTrue_ImagesOnly()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "Images", false, true, CancellationToken.None);

        response.Count().Should().Be(16);

        return Verify(response);
    }

    [Fact]
    public Task ReturnMatchingFilesFromTheRootFolderAndSubDirectoriesWhenRecursiveIsTrueAndIncludeSoftDeletedAndDeletePendingAreFalse_ImagesOnly()
    {
        var response = sut.Files.GetMatchingFiles("c:\\temp", true, "Images", false, false, CancellationToken.None);

        response.Count().Should().Be(14);

        return Verify(response);
    }
}
