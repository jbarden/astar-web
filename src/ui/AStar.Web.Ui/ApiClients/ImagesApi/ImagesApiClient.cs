using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using AStar.FilesApi.Client.SDK;
using AStar.FilesApi.Client.SDK.FilesApi;
using AStar.FilesApi.Client.SDK.Models;
using AStar.Web.UI.Shared;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Hosting.Server;

namespace AStar.Web.UI.ApiClients.ImagesApi;

public class ImagesApiClient(HttpClient httpClient, ILogger<ImagesApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

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
        var requestUri = $"api/image?thumbnail={thumbnail}&imagePath={Uri.EscapeDataString(imagePath)}&resize=true&maximumSizeInPixels={maximumSizeInPixels}";
        var response = await httpClient.GetAsync(requestUri);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadAsStreamAsync()
            : CreateNotFoundMemoryStream(imagePath);
    }

    private MemoryStream CreateNotFoundMemoryStream(string fileName)
    {
        logger.LogInformation("Could not find: {FileName}", fileName);

        return new(Models.NotFound.Image);
    }
}
