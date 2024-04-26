using System.Text.Json;
using AStar.Web.UI.Shared;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.Web.UI.ImagesApi;

public class ImagesApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly ILogger<ImagesApiClient> logger;

    public ImagesApiClient(HttpClient httpClient, ILogger<ImagesApiClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync("/health/live");

            return response.IsSuccessStatusCode
                ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(), JsonSerializerOptions))!
                : new() { Status = "Health Check failed." }!;
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);
            return new() { Status = "Could not get a response from the Images API." }!;
        }
    }
}
