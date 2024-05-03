using AStar.FilesApi.Files;

namespace AStar.FilesAPI.Files;

public class SearchParametersShould
{
    [Fact]
    public void GenerateTheExpectedToStringOutput()
    {
        var sut = new SearchParameters().ToString();

        sut.Should().Be("SearchFolder: ; SearchType: Images; Recursive: True; CurrentPage: 1; ItemsPerPage: 10; MaximumSizeOfThumbnail: 150; MaximumSizeOfImage: 1500; SortOrder: SizeDescending; SearchText: ");
    }

    [Fact]
    public void SetValueForCurrentPageWhenNotSpecified() => new SearchParameters().CurrentPage.Should().Be(1);

    [Fact]
    public void SetValueForItemsPerPageWhenNotSpecified() => new SearchParameters().ItemsPerPage.Should().Be(10);
}
