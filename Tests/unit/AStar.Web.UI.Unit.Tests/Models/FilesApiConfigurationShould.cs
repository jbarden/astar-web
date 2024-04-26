using AStar.Web.UI.Models;
using FluentAssertions;

namespace AStar.Web.UI.Unit.Tests.Models;

public class FilesApiConfigurationShould
{
    [Fact]
    public void ReturnTheExpectedDefaultValud() => new FilesApiConfiguration().BaseUrl.Should().Be("http://not.set.com/");
}
