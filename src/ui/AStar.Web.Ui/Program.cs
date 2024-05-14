using System.Diagnostics.CodeAnalysis;
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
        builder.CreateBootstrapLogger("astar-logging-settings.json");
        Log.Information("Starting: {AppName}", typeof(Program).AssemblyQualifiedName);
        _ = builder.AddLogging("astar-logging-settings.json");
        Log.Information("Starting2 {AppName}", typeof(Program).AssemblyQualifiedName);
        _ = builder.Services.Configure();

        Log.Information("Starting3 {AppName}", typeof(Program).AssemblyQualifiedName);
        _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

        Log.Information("Starting4 {AppName}", typeof(Program).AssemblyQualifiedName);
        var app = builder.Build();
        Log.Information("Starting5 {AppName}", typeof(Program).AssemblyQualifiedName);
        _ = ConfigurePipeline(app);

        Log.Information("Starting6 {AppName}", typeof(Program).AssemblyQualifiedName);
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
