using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.Logging.Extensions;
using Serilog;

namespace AStar.FilesApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        try
        {
            builder.CreateBootstrapLogger("astar-logging-settings.json");
            Log.Information("Starting {AppName}", typeof(Program).AssemblyQualifiedName);
            _ = builder.AddLogging("astar-logging-settings.json");
            _ = builder.Services.Configure();

            _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

            var app = builder.Build();
            _ = app.ConfigurePipeline();
            _ = ConfigurePipeline(app);

            app.Run();
        }
        catch(Exception ex)
        {
            Log.Error(ex, "Fatal error occurred in {AppName}", typeof(Program).AssemblyQualifiedName);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
        // Additional configuration can be performed here
        => app;
}
