using Microsoft.AspNetCore.Mvc.Testing;

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