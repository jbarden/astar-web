using AStar.Web.UI.Models;

namespace AStar.FilesAPI.Models;

public class FileInfoDtoShould
{
    [Fact]
    public void ContainTheExpectedProperties()
    {
        var sut = new FileInfoDto();

        sut.ToString().Should().Be(@"{""Name"":"""",""FullName"":"""",""Height"":0,""Width"":0,""Size"":0,""SizeForDisplay"":""0.00 Kb"",""ChecksumHash"":"""",""Created"":null,""SoftDeleted"":false,""DeletePending"":false}");
    }

    [Fact]
    public void ContainTheExpectedPropertiesIncludingTheFileSizeInKb()
    {
        var sut = new FileInfoDto() { Size = 123456 };

        sut.ToString().Should().Be(@"{""Name"":"""",""FullName"":"""",""Height"":0,""Width"":0,""Size"":123456,""SizeForDisplay"":""120.56 Kb"",""ChecksumHash"":"""",""Created"":null,""SoftDeleted"":false,""DeletePending"":false}");
    }

    [Fact]
    public void ContainTheExpectedPropertiesIncludingTheFileSizeInMb()
    {
        var sut = new FileInfoDto() { Size = 123456789 };

        sut.ToString().Should().Be(@"{""Name"":"""",""FullName"":"""",""Height"":0,""Width"":0,""Size"":123456789,""SizeForDisplay"":""117.74 Mb"",""ChecksumHash"":"""",""Created"":null,""SoftDeleted"":false,""DeletePending"":false}");
    }
}
