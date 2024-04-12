using System.IO.Abstractions;
using System.Text.Json.Serialization;
using AStar.ASPNet.Extensions;
using AStar.Clean.V1.Files.API.Services;
using AStar.Clean.V1.HealthChecks;
using AStar.Infrastructure.Data;
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
        _ = builder.Services.AddControllers()
                    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen();
        _ = builder.Services.AddHealthChecks();

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
        _ = app.UseMiddleware<GlobalExceptionMiddleware>()
               .UseSwagger()
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
