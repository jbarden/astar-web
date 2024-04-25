using System.Text.Json;
using AStar.Clean.V1.BlazorUI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AStar.Clean.V1.BlazorUI.ApiClients;

public class ImagesApiClient(HttpClient httpClient, FilesApiClient filesApiClient, ILogger<ImagesApiClient> logger)
{
    public async Task<Stream> GetImageThumbnailAsync(string imagePath, int maximumSizeInPixels)
    {
        var requestUri = $"/api/image?imagePath={imagePath}&thumbnail=true&maximumSizeInPixels={maximumSizeInPixels}";
        HttpResponseMessage response = null!;
        try
        {
            response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        return response?.IsSuccessStatusCode == true
            ? response.Content.ReadAsStreamAsync().Result
            : CreateNotFoundMemoryStream(imagePath);
    }

    public async Task<Stream> GetImageAsync(string imagePath, int maximumSizeInPixels)
    {
        var requestUri = $"/api/image?thumbnail=false&imagePath={imagePath}&resize=true&maximumSizeInPixels={maximumSizeInPixels}";
        var response = await httpClient.GetAsync(requestUri);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadAsStreamAsync()
            : CreateNotFoundMemoryStream(imagePath);
    }

    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        var response = await httpClient.GetAsync("/health/live");

        return !response.IsSuccessStatusCode
            ? new() { Status = $"{nameof(ImagesApiClient)} - Health Check failed" }
            : (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions(JsonSerializerDefaults.Web)))!;
    }

    public async Task<FileInfoDto?> GetImageDetailsAsync(string imagePath)
    {
        var requestUri = $"/api/image/details?imagePath={imagePath}";
        var response = await httpClient.GetAsync(requestUri);

        return response.IsSuccessStatusCode
            ? await JsonSerializer.DeserializeAsync<FileInfoDto>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions(JsonSerializerDefaults.Web))
            : new();
    }

    private MemoryStream CreateNotFoundMemoryStream(string fileName)
    {
        _ = filesApiClient.DeleteFileAsync(fileName, true);
        return new(File.ReadAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "404.jpg")));
    }
}
