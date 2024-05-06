namespace AStar.Web.UI.Configuration;

public class LoglevelShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new Loglevel().ToString().Should().Be("Default: Information; MicrosoftAspNetCore: Warning");
}
