using System.Text.Json;
using AStar.Web.UI.ApiClients.FilesApi;
using AStar.Web.UI.Shared;

namespace AStar.Web.UI.ApiClients.ImagesApi;

public class ImagesApiClient : IApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly FilesApiClient filesApiClient;
    private readonly ILogger<ImagesApiClient> logger;

    public ImagesApiClient(HttpClient httpClient, FilesApiClient filesApiClient, ILogger<ImagesApiClient> logger)
    {
        this.httpClient = httpClient;
        this.filesApiClient = filesApiClient;
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
            return new() { Status = "Could not get a response from the Images API." }!;
        }
    }

    public async Task<Stream> GetImageAsync(string imagePath, int maximumSizeInPixels, bool thumbnail)
    {
        var requestUri = $"/api/image?thumbnail={thumbnail}&imagePath={imagePath}&resize=true&maximumSizeInPixels={maximumSizeInPixels}";
        var response = await httpClient.GetAsync(requestUri);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadAsStreamAsync()
            : await CreateNotFoundMemoryStream(imagePath);
    }

    private async Task<MemoryStream> CreateNotFoundMemoryStream(string fileName)
    {
        logger.LogWarning("Could not find: {FileName}", fileName);
        _ = await filesApiClient.MarkForSoftDeletionAsync(fileName);

        return new(Models.NotFound.Image);
    }
}
