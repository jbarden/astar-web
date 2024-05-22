using AStar.Web.UI.Models;

namespace AStar.FilesAPI.Models;

public class FileInfoDtoShould
{
    [Fact]
    public Task ContainTheExpectedProperties()
    {
        var sut = new FileInfoDto();

        return Verify(sut);
    }

    [Fact]
    public Task ContainTheExpectedPropertiesIncludingTheFileSizeInKb()
    {
        var sut = new FileInfoDto() { Size = 123456 };

        return Verify(sut);
    }

    [Fact]
    public Task ContainTheExpectedPropertiesIncludingTheFileSizeInMb()
    {
        var sut = new FileInfoDto() { Size = 123456789 };

        return Verify(sut);
    }
}
