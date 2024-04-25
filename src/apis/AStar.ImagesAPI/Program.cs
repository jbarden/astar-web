using System.IO.Abstractions;
using AStar.ASPNet.Extensions.PipelineExtensions;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.ImagesAPI.ApiClients;
using AStar.ImagesAPI.Models;
using AStar.ImagesAPI.Services;
using AStar.Infrastructure.Data;
using Microsoft.Extensions.Options;

namespace AStar.ImagesAPI;

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

    private static IServiceCollection ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        _ = services.AddDbContext<FilesContext>();
        _ = services
                .AddSingleton<IFileSystem, FileSystem>()
                .AddSingleton<IImageService, ImageService>();

        _ = services.Configure<FilesApiConfiguration>(configuration.GetSection("ApiConfiguration:FilesApiConfiguration"));
        _ = services.AddHttpClient<FilesApiClient>().ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = serviceProvider.GetRequiredService<IOptions<FilesApiConfiguration>>().Value.BaseUrl;
                            client.DefaultRequestHeaders.Accept.Add(new("application/json"));
                        });

        return services;
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
        // Additional configuration can be performed here
        => app;
}
