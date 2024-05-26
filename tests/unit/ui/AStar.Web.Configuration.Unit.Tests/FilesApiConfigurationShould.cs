namespace AStar.Web.Configuration.Unit.Tests;

public class FilesApiConfigurationShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new FilesApiConfiguration().ToString().Should().Be(@"{""BaseUrl"":""https://not.set.com""}");
}
