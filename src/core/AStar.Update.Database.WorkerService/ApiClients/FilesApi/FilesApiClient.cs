using System.Text;
using System.Text.Json;
using AStar.Update.Database.WorkerService.Models;
using AStar.Utilities;

namespace AStar.Update.Database.WorkerService.ApiClients.FilesApi;

public class FilesApiClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<FilesApiClient> logger;

    public FilesApiClient(HttpClient httpClient, ILogger<FilesApiClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<FileAccessDetailDto> GetFileAccessDetail(int fileId)
    {
        var response = await httpClient.GetAsync($"api/files/access-detail?{fileId}");

        logger.LogInformation("Getting the access detail for the file with Id: {FileId}.", fileId);

        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content.FromJson<FileAccessDetailDto>(new(JsonSerializerDefaults.Web));
        }
        else
        {
            throw new InvalidOperationException("God won't give me a break...");
        }
    }

    public async Task<FileInfoDto> GetFileDetail(int fileId)
    {
        var response = await httpClient.GetAsync($"api/files/detail?{fileId}");

        logger.LogInformation("Getting the file detail for the file with Id: {FileId}.", fileId);

        if(response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content.FromJson<FileInfoDto>(new(JsonSerializerDefaults.Web));
        }
        else
        {
            throw new InvalidOperationException("God won't give me a break...");
        }
    }

    public async Task<string> MarkForSoftDeletionAsync(int fileId)
    {
        try
        {
            logger.LogInformation("Marking the {FileName} for soft deletion.", fileId);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-soft-deletion?request={fileId}");

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

    public async Task<string> UndoMarkForSoftDeletionAsync(int fileId)
    {
        try
        {
            logger.LogInformation("Unmarking the {FileName} for soft deletion.", fileId);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-soft-deletion?request={fileId}");

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

    public async Task<string> MarkForHardDeletionAsync(int fileId)
    {
        try
        {
            logger.LogInformation("Marking the {FileName} for hard deletion.", fileId);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-hard-deletion?request={fileId}");

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

    public async Task<string> UndoMarkForHardDeletionAsync(int fileId)
    {
        try
        {
            logger.LogInformation("Unmarking the {FileName} for hard deletion.", fileId);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-hard-deletion?request={fileId}");

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

    public async Task<string> MarkForMovingAsync(int fileId)
    {
        try
        {
            logger.LogInformation("Marking the {FileName} for moving.", fileId);
            var response = await httpClient.DeleteAsync($"/api/files/mark-for-moving?request={fileId}");

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

    public async Task<string> UndoMarkForMovingAsync(int fileId)
    {
        try
        {
            logger.LogInformation("Unmarking the {FileName} for moving.", fileId);
            var response = await httpClient.DeleteAsync($"/api/files/undo-mark-for-moving?request={fileId}");

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

    public async Task UpdateFileAsync(DirectoryChangeRequest directoryChangeRequest)
    {
        var httpContent = new StringContent(directoryChangeRequest.ToString(), Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"api/files/update-directory", httpContent);

        _ = response.EnsureSuccessStatusCode();

        logger.LogInformation("Update File {DirectoryChangeRequest} was {Status}", directoryChangeRequest.ToString(), response.StatusCode);
    }
}
