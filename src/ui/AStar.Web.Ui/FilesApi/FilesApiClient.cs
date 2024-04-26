using AStar.Web.UI.Shared;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.FilesApi;

public class FilesApiClient
{
    private readonly HttpClient httpClient;
    private readonly NullLogger<FilesApiClient> logger;

    public FilesApiClient(HttpClient httpClient, NullLogger<FilesApiClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<HealthStatusResponse> GetHealthAsync() => new() { Status = "Could not get a response from the Files API." };
}
