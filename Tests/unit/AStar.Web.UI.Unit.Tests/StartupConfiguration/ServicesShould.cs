using AStar.Web.UI.FilesApi;
using AStar.Web.UI.ImagesApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AStar.Web.UI.Unit.Tests.StartupConfiguration;

public class ServicesShould
{
    [Fact]
    public void AddExpectedFilesApiClient()
    {
        var builder = WebApplication.CreateBuilder([]);

        _ = UI.StartupConfiguration.Services.ConfigureServices(builder.Services, builder.Configuration);
        var provider = builder.Services.BuildServiceProvider();

        provider.GetService<FilesApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddExpectedImagesApiClient()
    {
        var builder = WebApplication.CreateBuilder([]);

        _ = UI.StartupConfiguration.Services.ConfigureServices(builder.Services, builder.Configuration);
        var provider = builder.Services.BuildServiceProvider();

        provider.GetService<ImagesApiClient>().Should().NotBeNull();
    }
}
