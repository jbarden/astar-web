using AStar.ASPNet.Extensions.Handlers;
using System.Text.Json.Serialization;
using AStar.Web.UI.FilesApi;
using AStar.Web.UI.Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using AStar.Web.UI.ImagesApi;

namespace AStar.Web.UI.Integration;

/// <summary>
/// See https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0 for details
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            
        });

        builder.UseEnvironment("Development");
    }
}