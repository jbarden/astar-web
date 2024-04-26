using AStar.ASPNet.Extensions.ServiceCollectionExtensions;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

namespace AStar.Web.UI;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.AddLogging("astar-logging-settings.json");
        _ = builder.Services.ConfigurePipeline();

        _ = ConfigureServices(builder.Services);

        var app = builder.Build();
        _ = ConfigurePipeline(app);

        app.Run();
    }

    private static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        _ = services.AddRazorPages();
        _ = services.AddServerSideBlazor();

        _ = services.AddBlazorise();
        _ = services.AddBootstrap5Providers()
            .AddFontAwesomeIcons();

        return services;
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
