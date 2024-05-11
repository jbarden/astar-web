using System.Text.Json;
using AStar.Web.UI.Shared;

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
            : CreateNotFoundMemoryStream(imagePath);
    }

    /// <summary>
    /// _ = filesApiClient.DeleteFileAsync(fileName, true);
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private MemoryStream CreateNotFoundMemoryStream(string fileName)
    {
        logger.LogWarning("Could not delete: {FileName}", fileName);
        return new(File.ReadAllBytes("404.jpg"));
    }
}
