namespace AStar.Web.UI.Configuration;

public class ApplicationSettingsShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new ApplicationSettings().ToString().Should().Be("DetailedErrors: False; Logging: LogLevel: Default: Information; MicrosoftAspNetCore: Warning; AllowedHosts: *.*; ApiConfiguration: FilesApiConfiguration: BaseUrl: https://not.set.com; ImagesApiConfiguration: BaseUrl: https://not.set.com; ApplicationConfiguration: PaginationPageDefaultPreAndPostCount: 0");
}
