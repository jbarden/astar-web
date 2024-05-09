namespace AStar.Web.UI.Configuration;

public class ImagesApiConfigurationShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new ImagesApiConfiguration().ToString().Should().Be("BaseUrl: https://not.set.com");
}
