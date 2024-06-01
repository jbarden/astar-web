using System.Text;

namespace AStar.Update.Database.WorkerService.ApiClients.FilesApi;

public class FilesApiClient(HttpClient httpClient, ILogger<FilesApiClient> logger)
{
    private readonly HttpClient httpClient = httpClient;
    private readonly ILogger<FilesApiClient> logger = logger;

    public async Task UpdateFileAsync(DirectoryChangeRequest directoryChangeRequest)
    {
        var httpContent = new StringContent(directoryChangeRequest.ToString(), Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"api/files/update-directory", httpContent);

        _ = response.EnsureSuccessStatusCode();

        logger.LogInformation("Update File {DirectoryChangeRequest} was {Status}", directoryChangeRequest.ToString(), response.StatusCode);
    }
}
