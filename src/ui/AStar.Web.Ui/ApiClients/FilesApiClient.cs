using AStar.GuardClauses;
using AStar.Web.Ui.Models;

namespace AStar.Web.Ui.ApiClients;

public class FilesApiClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<FilesApiClient> logger;

    public FilesApiClient(HttpClient httpClient, ILogger<FilesApiClient> logger)
    {
        this.httpClient = GuardAgainst.Null(httpClient);
        this.logger = GuardAgainst.Null(logger);
    }

    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        await Task.Delay(1);

        return new HealthStatusResponse() { Status = "Could not get a response from the Files API." };
    }
}
