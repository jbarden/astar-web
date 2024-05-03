using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;

namespace AStar.ASPNet.Extensions;

public class ServiceCollectionExtensionsShould
{
    [Fact]
    public void AddLoggingCorrectly()
    {
        var args = Array.Empty<string>();
        var webApplicationBuilder = WebApplication.CreateBuilder(args);

        webApplicationBuilder.AddLogging("astar-logging-settings.json");

        var provider = webApplicationBuilder.Services.BuildServiceProvider();
        var logger = provider.GetService<ILogger<ServiceCollectionExtensionsShould>>();
        logger.Should().NotBeNull();
    }
}
