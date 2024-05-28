using AStar.Infrastructure.Data;
using AStar.Update.Database.WorkerService.Models;
using Microsoft.EntityFrameworkCore;
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

        _ = builder.Services.AddSerilog(config => config.ReadFrom.Configuration(builder.Configuration));

        _ = builder.Services.AddHostedService<UpdateDatabaseForAllFiles>();
        _ = builder.Services.AddHostedService<DeleteMarkedFiles>();

        var host = builder.Build();

        using var context = new FilesContext(new DbContextOptionsBuilder<FilesContext>().UseSqlite("Data Source=F:\\files-db\\files.db").Options);
        _ = context.Database.EnsureCreated();

        host.Run();
    }
}
