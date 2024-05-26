namespace AStar.Web.Configuration.Unit.Tests;

public class ApiConfigurationShould
{
    [Fact]
    public void ContainTheExpectedDefaultPropertiesAndValues()
        => new ApiConfiguration().ToString().Should().Be(@"{""FilesApiConfiguration"":{""BaseUrl"":""https://not.set.com""},""ImagesApiConfiguration"":{""BaseUrl"":""https://not.set.com""}}");
}
