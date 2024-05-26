namespace AStar.Web.Configuration.Unit.Tests;

public class LoggingShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new Logging().ToString().Should().Be(@"{""LogLevel"":{""Default"":""Information"",""MicrosoftAspNetCore"":""Warning""}}");
}
