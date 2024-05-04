using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;

namespace AStar.FilesApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.AddLogging("astar-logging-settings.json");
        _ = builder.Services.Configure();

        _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

        var app = builder.Build();
        _ = app.ConfigurePipeline();
        _ = ConfigurePipeline(app);

        app.Run();
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
        // Additional configuration can be performed here
        => app;
}
