using System.Text.Json;
using AStar.Utilities;
using AStar.Web.UI.Models;
using AStar.Web.UI.Pages;
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

    public async Task<int> GetFilesCountAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/count?{searchParameters}");

        logger.LogWarning("Getting the count of matching files.");

        return response.IsSuccessStatusCode
            ? int.Parse(await response.Content.ReadAsStringAsync())
            : -1;
    }

    public async Task<int> GetDuplicateFilesCountAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/count-duplicates?{searchParameters}");

        logger.LogWarning("Getting the count of matching duplicate files.");

        return response.IsSuccessStatusCode
            ? int.Parse(await response.Content.ReadAsStringAsync())
            : -1;
    }

    public async Task<IEnumerable<FileInfoDto>> GetFilesAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/list?{searchParameters}");

        logger.LogWarning("Getting the list of files matching the criteria.");

        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content.FromJson<IEnumerable<FileInfoDto>>(new(JsonSerializerDefaults.Web));
        }
        else
        {
            throw new InvalidOperationException("God won't give me a break...");
        }
    }

    public async Task<IReadOnlyCollection<DuplicateGroup>> GetDuplicateFilesAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/list-duplicates?{searchParameters}");

        logger.LogWarning("Getting the list of duplicate files matching the criteria.");

        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content.FromJson<IReadOnlyCollection<DuplicateGroup>>(new(JsonSerializerDefaults.Web));
        }
        else
        {
            throw new InvalidOperationException("God won't give me a break...");
        }
    }

    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("/health/live");

            logger.LogWarning("Checking the FilesAPI Health.");
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

    public async Task<string> MarkForDeletionAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Marking the {FileName} for deletion.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-deletion?request={fullName}");

            return response.IsSuccessStatusCode
                            ? "Marked for deletion"
                            : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);

            return ex.Message;
        }
    }

    public async Task<string> UndoMarkForDeletionAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Unmarking the {FileName} for deletion.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-deletion?request={fullName}");

            return response.IsSuccessStatusCode
                ? "Mark for deletion has been undone"
                : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);
            return ex.Message;
        }
    }
}
