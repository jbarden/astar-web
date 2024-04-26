using System.Text.Json;
using AStar.Web.UI.Shared;

namespace AStar.Web.UI.FilesApi;

public class FilesApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly ILogger<FilesApiClient> logger;

    public FilesApiClient(HttpClient httpClient, ILogger<FilesApiClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("/health/live");

            return response.IsSuccessStatusCode
                ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(), JsonSerializerOptions))!
                : new() { Status = "Health Check failed." }!;
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);
            return new() { Status = "Could not get a response from the Files API." }!;
        }
    }
}
