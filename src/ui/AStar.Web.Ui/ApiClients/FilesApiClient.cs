using System.Text;
using System.Text.Json;
using AStar.FilesApi.Client.SDK.Models;
using AStar.Utilities;
using Microsoft.Extensions.Logging;

namespace AStar.FilesApi.Client.SDK.FilesApi;

/// <summary>
/// The <see href="FilesApiClient"></see> class.
/// </summary>
public class FilesApiClient(HttpClient httpClient, ILogger<FilesApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <inheritdoc/>
    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("/health/live");

            logger.LogInformation("Checking the FilesAPI Health.");
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

    /// <summary>
    /// The GetFilesCountAsync method will get the count of the files that match the search parameters.
    /// </summary>
    /// <param name="searchParameters">An instance of the <see href="SearchParameters"></see> class defining the search criteria for the files count.</param>
    /// <returns>The count of the matching files or -1 if an error occurred.</returns>
    public async Task<int> GetFilesCountAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/count?{searchParameters.ToQueryString()}");

        logger.LogInformation("Getting the count of matching files.");

        return response.IsSuccessStatusCode
                            ? int.Parse(await response.Content.ReadAsStringAsync())
                            : -1;
    }

    /// <summary>
    /// The GetDuplicateFilesCountAsync method will get the count of the duplicate files that match the search parameters.
    /// </summary>
    /// <param name="searchParameters">An instance of the <see href="SearchParameters"></see> class defining the search criteria for the duplicate files count.</param>
    /// <returns>The count of the matching duplicate files or -1 if an error occurred.</returns>
    public async Task<int> GetDuplicateFilesCountAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/count-duplicates?{searchParameters.ToQueryString()}");

        logger.LogInformation("Getting the count of matching duplicate files.");

        return response.IsSuccessStatusCode
                            ? int.Parse(await response.Content.ReadAsStringAsync())
                            : -1;
    }

    /// <summary>
    /// The GetFilesAsync method will, as its name suggests, get the files that match the search parameters.
    /// </summary>
    /// <param name="searchParameters">An instance of the <see href="SearchParameters"></see> class defining the search criteria for the files search.</param>
    /// <returns>An enumerable list of <see href="FileDetail"></see> instances.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<IEnumerable<FileDetail>> GetFilesAsync(SearchParameters searchParameters)
    {
        var response = await httpClient.GetAsync($"api/files/list?{searchParameters.ToQueryString()}");

        logger.LogInformation("Getting the list of files matching the criteria.");
        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? content.FromJson<IEnumerable<FileDetail>>(new(JsonSerializerDefaults.Web))
            : throw new InvalidOperationException(content);
    }

    /// <summary>
    /// The GetDuplicateFilesAsync method will, as its name suggests, get the duplicate files that match the search parameters.
    /// </summary>
    /// <param name="searchParameters">An instance of the <see href="SearchParameters"></see> class defining the search criteria for the duplicate files search.</param>
    /// <returns>An enumerable list of <see href="DuplicateGroup"></see> instances.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<IReadOnlyCollection<DuplicateGroup>> GetDuplicateFilesAsync(SearchParameters searchParameters)
    {
        var queryString = searchParameters.ToQueryString();
        var response = await httpClient.GetAsync($"api/files/list-duplicates?{queryString}");

        logger.LogInformation("Getting the list of duplicate files matching the criteria.");
        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? content.FromJson<IReadOnlyCollection<DuplicateGroup>>(new(JsonSerializerDefaults.Web))
            : throw new InvalidOperationException(content);
    }

    /// <summary>
    /// The GetFileAccessDetail method will, as its name suggests, get the file access details for the specified file.
    /// </summary>
    /// <param name="fileId">The Id of the file to retrieve the File Access Details from the database.</param>
    /// <returns>An instance of <see href="FileAccessDetail"></see> for the specified File Id.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FileAccessDetail> GetFileAccessDetail(Guid fileId)
    {
        var response = await httpClient.GetAsync($"api/files/access-detail?request={fileId}");

        logger.LogInformation("Getting the access detail for the file with Id: {FileId}.", fileId);
        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? content.FromJson<FileAccessDetail>(new(JsonSerializerDefaults.Web))
            : throw new InvalidOperationException(content);
    }

    /// <summary>
    /// The GetFileDetail method will, as its name suggests, get the file details of the specified file.
    /// </summary>
    /// <param name="fileId">The Id of the file detail to retrieve from the database.</param>
    /// <returns>An awaitable task containing an instance of <see href="FileDetail"></see> containing the, you guessed it, File details...</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<FileDetail> GetFileDetail(Guid fileId)
    {
        var response = await httpClient.GetAsync($"api/files/detail?request={fileId}");

        logger.LogInformation("Getting the file detail for the file with Id: {FileId}.", fileId);
        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode
            ? content.FromJson<FileDetail>(new(JsonSerializerDefaults.Web))
            : throw new InvalidOperationException(content);
    }

    /// <summary>
    /// The MarkForSoftDeletionAsync method will, as its name suggests, mark the specified file as soft deleted.
    /// </summary>
    /// <param name="fileId">The Id of the file to mark as soft deleted.</param>
    /// <returns>An awaitable task containing a string with the status of the update.</returns>
    public async Task<string> MarkForSoftDeletionAsync(Guid fileId)
    {
        try
        {
            logger.LogInformation("Marking the {FileName} for soft deletion.", fileId);
            var content = new StringContent(JsonSerializer.Serialize(new { id = fileId }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/files/mark-for-soft-deletion", content);

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

    /// <summary>
    /// The UndoMarkForSoftDeletionAsync method will, as its name suggests, unmark the specified file as soft deleted.
    /// </summary>
    /// <param name="fileId">The Id of the file to unmark as soft deleted.</param>
    /// <returns>An awaitable task containing a string with the status of the update.</returns>
    public async Task<string> UndoMarkForSoftDeletionAsync(Guid fileId)
    {
        try
        {
            logger.LogInformation("Unmarking the {FileName} for soft deletion.", fileId);
            var content = new StringContent(JsonSerializer.Serialize(new { id = fileId }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/files/undo-mark-for-soft-deletion", content);

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

    /// <summary>
    /// The MarkForHardDeletionAsync method will, as its name suggests, mark the specified file as hard deleted.
    /// </summary>
    /// <param name="fileId">The Id of the file to mark as hard deleted.</param>
    /// <returns>An awaitable task containing a string with the status of the update.</returns>
    public async Task<string> MarkForHardDeletionAsync(Guid fileId)
    {
        try
        {
            logger.LogInformation("Marking the {FileName} for hard deletion.", fileId);
            var content = new StringContent(JsonSerializer.Serialize(new { id = fileId }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/files/mark-for-hard-deletion", content);

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

    /// <summary>
    /// The UndoMarkForHardDeletionAsync method will, as its name suggests, unmark the specified file as hard deleted.
    /// </summary>
    /// <param name="fileId">The Id of the file to unmark as hard deleted.</param>
    /// <returns>An awaitable task containing a string with the status of the update.</returns>
    public async Task<string> UndoMarkForHardDeletionAsync(Guid fileId)
    {
        try
        {
            logger.LogInformation("Unmarking the {FileName} for hard deletion.", fileId);
            var content = new StringContent(JsonSerializer.Serialize(new { id = fileId }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/files/undo-mark-for-hard-deletion", content);

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

    /// <summary>
    /// The MarkForMovingAsync method will, as its name suggests, mark the specified file as requiring moving.
    /// </summary>
    /// <param name="fileId">The Id of the file to mark as move required.</param>
    /// <returns>An awaitable task containing a string with the status of the update.</returns>
    public async Task<string> MarkForMovingAsync(Guid fileId)
    {
        try
        {
            logger.LogInformation("Marking the {FileName} for moving.", fileId);
            var content = new StringContent(JsonSerializer.Serialize(new { id = fileId }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/files/mark-for-moving", content);

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

    /// <summary>
    /// The UndoMarkForMovingAsync method will, as its name suggests, unmark the specified file as requiring moving.
    /// </summary>
    /// <param name="fileId">The Id of the file to unmark as move required.</param>
    /// <returns>An awaitable task containing a string with the status of the update.</returns>
    public async Task<string> UndoMarkForMovingAsync(Guid fileId)
    {
        try
        {
            logger.LogInformation("Unmarking the {FileName} for moving.", fileId);
            var content = new StringContent(JsonSerializer.Serialize(new { id = fileId }), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"/api/files/undo-mark-for-moving", content);

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

    /// <summary>
    /// The UpdateFileAsync method will, as the name suggests, update the file - currently, the directory is the only thing to change.
    /// </summary>
    /// <param name="directoryChangeRequest">An instance of the <see href="DirectoryChangeRequest"></see> class used to control the file update.</param>
    /// <returns>An awaitable task.</returns>
    public async Task<string> UpdateFileAsync(DirectoryChangeRequest directoryChangeRequest)
    {
        var httpContent = new StringContent(directoryChangeRequest.ToString(), Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"api/files/update-directory", httpContent);

        _ = response.EnsureSuccessStatusCode();

        logger.LogInformation("Update File {DirectoryChangeRequest} response was: {Status}", directoryChangeRequest.ToString(), response.StatusCode);

        return response.IsSuccessStatusCode
                        ? "The file details were updated successfully"
                        : await response.Content.ReadAsStringAsync();
    }
}
