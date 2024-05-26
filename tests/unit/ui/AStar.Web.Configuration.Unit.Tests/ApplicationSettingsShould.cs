namespace AStar.Web.Configuration.Unit.Tests;

public class ApplicationSettingsShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new ApplicationSettings().ToString().Should().Be(@"{""DetailedErrors"":false,""Logging"":{""LogLevel"":{""Default"":""Information"",""MicrosoftAspNetCore"":""Warning""}},""AllowedHosts"":""*.*"",""ApiConfiguration"":{""FilesApiConfiguration"":{""BaseUrl"":""https://not.set.com""},""ImagesApiConfiguration"":{""BaseUrl"":""https://not.set.com""}},""ApplicationConfiguration"":{""PaginationPageDefaultPreAndPostCount"":0}}");
}
