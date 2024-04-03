using System.IO.Abstractions;
using AStar.Clean.V1.Files.API.Services;
using AStar.Clean.V1.HealthChecks;
using AStar.Infrastructure.Data.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AStar.Clean.V1.Files.API;

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
        _ = services
            .AddControllers();
        _ = services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHealthChecks();

        _ = builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}")
            .ReadFrom.Configuration(context.Configuration));

        _ = services.AddDbContext<FilesContext>();
        _ = services.AddSwaggerGenNewtonsoftSupport();
        _ = services.AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton<IImageService, ImageService>();
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
