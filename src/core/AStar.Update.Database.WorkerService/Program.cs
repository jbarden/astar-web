using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace AStar.Update.Database.WorkerService;

internal class Program
{
    protected Program()
    { }

    private static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()                    .AddJsonFile("appsettings.json")                    .Build();

        var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
        logger.Information("Starting up");

        var builder = Host.CreateApplicationBuilder(args);
        _ = builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}
