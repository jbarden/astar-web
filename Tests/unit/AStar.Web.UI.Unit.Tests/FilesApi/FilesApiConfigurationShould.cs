using AStar.Web.UI.FilesApi;

namespace AStar.Web.UI.Unit.Tests.FilesApi;

public class FilesApiConfigurationShould
{
    [Fact]
    public void ReturnTheExpectedDefaultValud() => new FilesApiConfiguration().BaseUrl.Should().Be("http://not.set.com/");

    [Fact]
    public void ReturnTheExpectedSectionLocationValud() => FilesApiConfiguration.SectionLocation.Should().Be("ApiConfiguration:FilesApiConfiguration");
}
