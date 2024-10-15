using AStar.FilesApi.Client.SDK.Models;

namespace AStar.Web.UI.Models;

public class FileInfoDtoShould
{
    [Fact]
    public Task ContainTheExpectedProperties()
    {
        var sut = new FileDetail();

        return Verify(sut);
    }

    [Fact]
    public Task ContainTheExpectedPropertiesIncludingTheFileSizeInKb()
    {
        var sut = new FileDetail() { Size = 123456 };

        return Verify(sut);
    }

    [Fact]
    public Task ContainTheExpectedPropertiesIncludingTheFileSizeInMb()
    {
        var sut = new FileDetail() { Size = 123456789 };

        return Verify(sut);
    }
}
