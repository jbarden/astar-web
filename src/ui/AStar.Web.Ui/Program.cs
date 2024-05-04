using System.Diagnostics.CodeAnalysis;
using AStar.ASPNet.Extensions.ServiceCollectionExtensions;

namespace AStar.Web.UI;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.AddLogging("astar-logging-settings.json");
        _ = builder.Services.Configure();

        _ = StartupConfiguration.Services.Configure(builder.Services, builder.Configuration);

        var app = builder.Build();
        _ = ConfigurePipeline(app);

        app.Run();
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
