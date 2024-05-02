namespace AStar.Web.UI.FilesApi;

public class FilesApiConfigurationShould
{
    [Fact]
    public void ReturnTheExpectedDefaultValud() => new FilesApiConfiguration().BaseUrl.Should().Be("http://not.set.com/");

    [Fact]
    public void ReturnTheExpectedSectionLocationValud() => FilesApiConfiguration.SectionLocation.Should().Be("ApiConfiguration:FilesApiConfiguration");
}
