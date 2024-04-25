using System.IdentityModel.Tokens.Jwt;
using AStar.Clean.V1.BlazorUI.ApiClients;
using AStar.Clean.V1.BlazorUI.Models;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Serilog;

namespace AStar.Clean.V1.BlazorUI;

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

    private static void ConfigurePipeline(WebApplication app)
    {
        _ = app.UseExceptionHandler("/Error");
        _ = app.UseHsts();

        _ = app.UseHttpsRedirection();
        _ = app.UseStaticFiles();
        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage("/_Host");
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        _ = services.AddRazorPages();
        _ = services.AddServerSideBlazor();
        _ = services.AddBootstrapBlazor();
        _ = services.AddApplicationInsightsTelemetry();
        _ = builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}")
            .ReadFrom.Configuration(context.Configuration));
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        _ = services.AddHttpContextAccessor();

        _ = services.AddControllersWithViews().AddMicrosoftIdentityUI();

        _ = builder.Services.Configure<FilesApiConfiguration>(
            builder.Configuration.GetSection("ApiConfiguration:FilesApiConfiguration"));
        _ = builder.Services.Configure<ImagesApiConfiguration>(
            builder.Configuration.GetSection("ApiConfiguration:ImagesApiConfiguration"));
        _ = services.AddHttpClient<FilesApiClient>().ConfigureHttpClient((serviceProvider, client) =>
        {
            client.BaseAddress = serviceProvider.GetRequiredService<IOptions<FilesApiConfiguration>>().Value.BaseUrl;
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));
        });
        _ = services.AddHttpClient<ImagesApiClient>().ConfigureHttpClient((serviceProvider, client) =>
        {
            client.BaseAddress = serviceProvider.GetRequiredService<IOptions<ImagesApiConfiguration>>().Value.BaseUrl;
            client.DefaultRequestHeaders.Accept.Add(new("application/json"));
        });

        _ = services.AddRazorPages();

        _ = services.AddServerSideBlazor()
            .AddMicrosoftIdentityConsentHandler();
    }
}
