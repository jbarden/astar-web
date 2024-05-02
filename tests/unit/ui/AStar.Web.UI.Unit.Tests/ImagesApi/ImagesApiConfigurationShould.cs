namespace AStar.Web.UI.ImagesApi;

public class ImagesApiConfigurationShould
{
    [Fact]
    public void ReturnTheExpectedDefaultValud() => new ImagesApiConfiguration().BaseUrl.Should().Be("http://not.set.com/");

    [Fact]
    public void ReturnTheExpectedSectionLocationValud() => ImagesApiConfiguration.SectionLocation.Should().Be("ApiConfiguration:ImagesApiConfiguration");
}
