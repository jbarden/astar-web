using System.Diagnostics.CodeAnalysis;
using AStar.Api.HealthChecks;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using AStar.Logging.Extensions;
using Serilog;

namespace AStar.Web.UI;

[ExcludeFromCodeCoverage]
public class Program
{
    protected Program()
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        try
        {
            var app = RunConfiguration(builder);
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

    private static WebApplication RunConfiguration(WebApplicationBuilder builder)
    {
        _ = builder.CreateBootstrapLogger("astar-logging-settings.json")
                   .AddLogging("astar-logging-settings.json")
                   .Services.ConfigureUi();

        _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

        var app = builder.Build();
        Log.Information("Starting {AppName}", typeof(Program).AssemblyQualifiedName);
        _ = ConfigurePipeline(app);

        return app;
    }

    private static WebApplication ConfigurePipeline(WebApplication app)
    {
        if(!app.Environment.IsDevelopment())
        {
            _ = app.UseHsts();
        }

        _ = app.UseExceptionHandler("/Error");
        _ = app.UseHttpsRedirection();

        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage("/_Host");

        return app;
    }
}
