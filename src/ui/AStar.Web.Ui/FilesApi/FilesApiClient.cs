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

    public async Task<string> MarkForSoftDeletionAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Marking the {FileName} for soft deletion.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-soft-deletion?request={fullName}");

            return response.IsSuccessStatusCode
                            ? "Marked for soft deletion"
                            : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);

            return ex.Message;
        }
    }

    public async Task<string> UndoMarkForSoftDeletionAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Unmarking the {FileName} for soft deletion.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-soft-deletion?request={fullName}");

            return response.IsSuccessStatusCode
                ? "Mark for soft deletion has been undone"
                : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);
            return ex.Message;
        }
    }

    public async Task<string> MarkForHardDeletionAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Marking the {FileName} for hard deletion.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-hard-deletion?request={fullName}");

            return response.IsSuccessStatusCode
                            ? "Marked for hard deletion."
                            : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);

            return ex.Message;
        }
    }

    public async Task<string> UndoMarkForHardDeletionAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Unmarking the {FileName} for hard deletion.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-hard-deletion?request={fullName}");

            return response.IsSuccessStatusCode
                            ? "Mark for hard deletion has been undone"
                            : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);

            return ex.Message;
        }
    }

    public async Task<string> MarkForMovingAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Marking the {FileName} for moving.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-moving?request={fullName}");

            return response.IsSuccessStatusCode
                            ? "Mark for moving was successful"
                            : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);

            return ex.Message;
        }
    }

    public async Task<string> UndoMarkForMovingAsync(string fullName)
    {
        try
        {
            logger.LogWarning("Unmarking the {FileName} for moving.", fullName);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-moving?request={fullName}");

            return response.IsSuccessStatusCode
                            ? "Undo mark for moving was successful"
                            : await response.Content.ReadAsStringAsync();
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);

            return ex.Message;
        }
    }
}
