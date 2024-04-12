using System.IO.Abstractions;
using System.Text.Json.Serialization;
using AStar.ASPNet.Extensions;
using AStar.Clean.V1.HealthChecks;
using AStar.Clean.V1.Images.API.ApiClients;
using AStar.Clean.V1.Images.API.Models;
using AStar.Clean.V1.Images.API.Services;
using AStar.Infrastructure.Data;
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
        _ = services.AddDbContext<FilesContext>();
        _ = builder.Services.AddControllers()
                    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = builder.Services.AddEndpointsApiExplorer();
        _ = builder.Services.AddSwaggerGen();
        _ = builder.Services.AddHealthChecks();
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
        _ = app
            .UseMiddleware<GlobalExceptionMiddleware>()
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
