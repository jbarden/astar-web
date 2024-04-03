using System.IO.Abstractions;
using AStar.Clean.V1.HealthChecks;
using AStar.Clean.V1.Images.API.ApiClients;
using AStar.Clean.V1.Images.API.Models;
using AStar.Clean.V1.Images.API.Services;
using AStar.Clean.V1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace AStar.Clean.V1.Images.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);

        var app = builder.Build();

        ConfigurePipeline(app);

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        _ = builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}")
            .ReadFrom.Configuration(context.Configuration));
        _ = services.AddControllers();
        _ = services.AddDbContext<FilesDbContext>(options =>
            options.UseSqlite(
                $@"Data Source={Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Files.db")}"));
        _ = services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHealthChecks();
        _ = services
            .AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton<IImageService, ImageService>();
        _ = builder.Services.Configure<FilesApiConfiguration>(
            builder.Configuration.GetSection("ApiConfiguration:FilesApiConfiguration"));
        _ = services.AddHttpClient<FilesApiClient>().ConfigureHttpClient((serviceProvider, client) =>
        {
            client.BaseAddress = serviceProvider.GetRequiredService<IOptions<FilesApiConfiguration>>().Value.BaseUrl;
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));
        });
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        _ = app.UseSwagger()
            .UseSwaggerUI()
            .UseAuthentication()
            .UseAuthorization();

        _ = app.MapControllers();
        _ = app.MapHealthChecks("/health/live", new()
        {
            Predicate = _ => false,
            ResponseWriter = HealthCheckResponses.WriteJsonResponse
        });
        _ = app.MapHealthChecks("/health/ready", new()
        {
            ResponseWriter = HealthCheckResponses.WriteJsonResponse
        });
    }
}
