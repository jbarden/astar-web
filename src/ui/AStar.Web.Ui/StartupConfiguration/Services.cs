using AStar.Web.UI.ApiClients.FilesApi;
using AStar.Web.UI.ApiClients.ImagesApi;
using AStar.Web.UI.Services;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.Options;

namespace AStar.Web.UI.StartupConfiguration;

public static class Services
{
    public static IServiceCollection Configure(IServiceCollection services, ConfigurationManager configuration)
    {
        _ = services.AddOptions<FilesApiConfiguration>()
                    .Bind(configuration.GetSection(FilesApiConfiguration.SectionLocation))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
        _ = services.AddOptions<ImagesApiConfiguration>()
                    .Bind(configuration.GetSection(ImagesApiConfiguration.SectionLocation))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

        _ = services.AddRazorPages();
        _ = services.AddServerSideBlazor();
        _ = services.AddScoped<PaginationService>();
        _ = services.AddScoped<SearchFilesService>();

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
