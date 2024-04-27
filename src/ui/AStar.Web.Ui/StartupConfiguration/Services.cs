using AStar.Web.UI.FilesApi;
using AStar.Web.UI.ImagesApi;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.Options;

namespace AStar.Web.UI.StartupConfiguration;

public static class Services
{
    public static IServiceCollection ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
    {
        _ = services.Configure<FilesApiConfiguration>(configuration.GetSection(FilesApiConfiguration.SectionLocation));
        _ = services.Configure<ImagesApiConfiguration>(configuration.GetSection(ImagesApiConfiguration.SectionLocation));
        _ = services.AddRazorPages();
        _ = services.AddServerSideBlazor();

        _ = services.AddBlazorise()
                    .AddBootstrap5Providers()
                    .AddFontAwesomeIcons();

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

        return services;
    }
}
