using AStar.FilesApi.Files;

namespace AStar.FilesAPI.Files;

public class CountDuplicatesSearchParametersShould
{
    [Fact]
    public void GenerateTheExpectedToStringOutput()
    {
        var sut = new CountDuplicatesSearchParameters().ToString();

        sut.Should().Be(@"{""SearchFolder"":"""",""Recursive"":true,""IncludeSoftDeleted"":false,""IncludeMarkedForDeletion"":false,""SearchText"":null}");
    }
}
