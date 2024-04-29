using System.IO.Abstractions;
using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.Infrastructure.Data;

namespace AStar.FilesApi;

public static class Program
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

    private static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped(_ => new FilesContext(configuration["FilesDb"]!));
        _ = services.AddSwaggerGenNewtonsoftSupport();
        _ = services.AddSingleton<IFileSystem, FileSystem>();

        return services;
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
        // Additional configuration can be performed here
        => app;
}
