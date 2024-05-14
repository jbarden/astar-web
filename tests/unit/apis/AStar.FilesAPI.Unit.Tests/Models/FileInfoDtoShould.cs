using AStar.FilesApi.Models;

namespace AStar.FilesAPI.Models;

public class FileInfoDtoShould
{
    [Fact]
    public void ContainTheExpectedProperties()
    {
        var sut = new FileInfoDto();

        sut.ToString().Should().Be(@"{""Name"":"""",""FullName"":"""",""Height"":0,""Width"":0,""Size"":0,""DetailsLastUpdated"":null,""LastViewed"":null,""Extension"":"""",""SoftDeleted"":false,""DeletePending"":false}");
    }
}
