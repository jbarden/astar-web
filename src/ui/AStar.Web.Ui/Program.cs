using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

namespace AStar.Web.UI;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services.AddRazorPages();
        _ = builder.Services.AddServerSideBlazor();

        AddBlazorise(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if(!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage("/_Host");

        app.Run();

        static void AddBlazorise(IServiceCollection services)
        {
            _ = services
                .AddBlazorise();
            _ = services
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();
        }
    }
}
