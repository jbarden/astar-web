using AStar.FilesApi.Client.SDK.FilesApi;

namespace AStar.Web.UI.ApiClients.FilesApiClient;

public class FilesApiConfigurationShould
{
    [Fact]
    public void ReturnTheExpectedDefaultValud() => new FilesApiConfiguration().BaseUrl.Should().Be("http://not.set.com/");

    [Fact]
    public void ReturnTheExpectedSectionLocationValud() => FilesApiConfiguration.SectionLocation.Should().Be("ApiConfiguration:FilesApiConfiguration");
}
