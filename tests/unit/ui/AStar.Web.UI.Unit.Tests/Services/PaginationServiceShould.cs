namespace AStar.Web.UI.Services;

public class PaginationServiceShould
{
    private readonly PaginationService sut;

    public PaginationServiceShould() => sut = new PaginationService();

    [Fact]
    public void ProduceAnEmptyResponseWhenNoPagesExistToPaginate()
    {
        const int pageCount = 0;

        sut.GetPaginationInformation(pageCount, pageCount).Count.Should().Be(0);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(4, 4)]
    [InlineData(10, 10)]
    public void ProduceExpectedResponseWhenPageCountIs10OrLess(int pageCount, int expectedCount)
        => sut.GetPaginationInformation(pageCount, pageCount).Count.Should().Be(expectedCount);

    [Fact]
    public void ProduceTheExpected10EntriesWhenThePageCountIs11()
    {
        const int pageCount = 11;

        sut.GetPaginationInformation(pageCount, pageCount).Should().BeEquivalentTo([1, 2, 3, 4, 5, 7, 8, 9, 10, 11]);
    }

    [Fact]
    public void ProduceTheExpectedEntriesWhenThePageCountIs111AndCurrentPageIs50()
    {
        const int pageCount = 111;

        sut.GetPaginationInformation(pageCount, 50).Should().BeEquivalentTo([1, 2, 3, 4, 5, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 107, 108, 109, 110, 111]);
    }
}
