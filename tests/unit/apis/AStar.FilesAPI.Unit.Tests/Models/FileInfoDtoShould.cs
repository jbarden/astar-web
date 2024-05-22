using AStar.FilesApi.Models;

namespace AStar.FilesAPI.Models;

public class FileInfoDtoShould
{
    [Fact]
    public Task ContainTheExpectedProperties()
    {
        var sut = new FileInfoDto();

        return Verify(sut);
    }
}
