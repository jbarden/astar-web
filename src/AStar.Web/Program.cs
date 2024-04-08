using AStar.Web.ApiClients;
using AStar.Web.Components;
using AStar.Web.Models;
using Microsoft.Extensions.Options;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var services = builder.Services;
        _ = services.AddBootstrapBlazor();

        _ = builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}")
            .ReadFrom.Configuration(context.Configuration));

        _ = services.AddHttpContextAccessor();

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
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if(!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}