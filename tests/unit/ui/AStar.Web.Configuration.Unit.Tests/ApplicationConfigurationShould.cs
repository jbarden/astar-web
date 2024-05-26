namespace AStar.Web.Configuration.Unit.Tests;

public class ApplicationConfigurationShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new ApplicationConfiguration().ToString().Should().Be(@"{""PaginationPageDefaultPreAndPostCount"":0}");
}
