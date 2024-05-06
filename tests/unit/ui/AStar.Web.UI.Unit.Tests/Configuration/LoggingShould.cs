namespace AStar.Web.UI.Configuration;

public class LoggingShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new Logging().ToString().Should().Be("LogLevel: Default: Information; MicrosoftAspNetCore: Warning");
}
