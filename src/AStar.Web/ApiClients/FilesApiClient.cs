using System.Text.Json;
using AStar.Web.Models;

namespace AStar.Web.ApiClients;

public class FilesApiClient
{
    private readonly HttpClient httpClient;

    public FilesApiClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.httpClient.Timeout = TimeSpan.FromMinutes(4);
    }

    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        var response = await httpClient.GetAsync("/health/live");

        return response.IsSuccessStatusCode
            ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions(JsonSerializerDefaults.Web)))!
            : new() { Status = "Health Check failed" }!;
    }

    public async Task<IList<FileInfoDto>> GetFilesListAsync(SearchParameters searchParameters, CancellationToken cancellationToken)
    {
        searchParameters.CountOnly = false;
        var requestUri = $"/api/files?{searchParameters}";
        using var response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        _ = response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<IList<FileInfoDto>>(cancellationToken: cancellationToken))!;
    }

    public async Task<int> GetFilesCountAsync(SearchParameters searchParameters, CancellationToken cancellationToken)
    {
        searchParameters.CountOnly = true;
        var requestUri = $"/api/filesCount?{searchParameters}";
        var response = await httpClient.GetAsync(requestUri, cancellationToken);
        if(!response.IsSuccessStatusCode)
        {
            return 0;
        }

        var content = await response.Content.ReadAsStreamAsync(cancellationToken);
        var filesCount = await JsonSerializer.DeserializeAsync<int>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken);

        return filesCount;
    }

    public async Task<HttpResponseMessage> DeleteFileAsync(string fullname, bool hardDelete)
    {
        var requestUri = $"/api/files?filePath={fullname}&hardDelete={hardDelete}";
        var response = await httpClient.DeleteAsync(requestUri);

        return response;
    }
}
