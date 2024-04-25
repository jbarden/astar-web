using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.Web.Ui.Models;

namespace AStar.Web.Ui;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.AddLogging("astar-logging-settings.json");
        _ = builder.Services.ConfigurePipeline();

        _ = ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        _ = app.ConfigurePipeline();
        _ = ConfigurePipeline(app);

        app.Run();
    }

    private static IServiceCollection ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        _ = services.Configure<FilesApiConfiguration>(configuration.GetSection("ApiConfiguration:FilesApiConfiguration"));

        return services;
    }

    private static WebApplication ConfigurePipeline(WebApplication app) => app;
}
