using AStar.Infrastructure.Data;
using AStar.Update.Database.WorkerService.ApiClients.FilesApi;
using AStar.Update.Database.WorkerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace AStar.Update.Database.WorkerService;

internal class Program
{
    protected Program()
    { }

    private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .AddUserSecrets<Program>()
                                    .Build();

        var logger = new LoggerConfiguration()
                                .ReadFrom.Configuration(configuration)
                                .CreateLogger();

        Log.Logger = logger;
        logger.Information("AStar.Update.Database.WorkerService Starting up");

        var builder = Host.CreateApplicationBuilder(args);

        _ = builder.Services.AddOptions<ApiConfiguration>()
                            .Bind(configuration.GetSection(ApiConfiguration.SectionLocation))
                            .ValidateOnStart();
        _ = builder.Services.AddOptions<DirectoryChanges>()
                            .Bind(configuration.GetSection(DirectoryChanges.SectionLocation))
                            .ValidateOnStart();
        _ = builder.Services.AddOptions<FilesApiConfiguration>()
                            .Bind(configuration.GetSection(FilesApiConfiguration.SectionLocation))
                            .ValidateDataAnnotations()
                            .ValidateOnStart();

        _ = builder.Services.AddHttpClient<FilesApiClient>().ConfigureHttpClient((serviceProvider, client) =>
                            {
                                client.BaseAddress = serviceProvider.GetRequiredService<IOptions<FilesApiConfiguration>>().Value.BaseUrl;
                                client.DefaultRequestHeaders.Accept.Add(new("application/json"));
                            });

        _ = builder.Services.AddSerilog(config => config.ReadFrom.Configuration(builder.Configuration));

        _ = builder.Services.AddHostedService<UpdateDatabaseForAllFiles>()
                            .AddHostedService<DeleteMarkedFiles>()
                            .AddHostedService<MoveFiles>();

        var host = builder.Build();

        using var context = new FilesContext(new DbContextOptionsBuilder<FilesContext>().UseSqlite(ServiceConstants.SqliteConnectionString).Options);
        _ = context.Database.EnsureCreated();

        host.Run();
    }
}
