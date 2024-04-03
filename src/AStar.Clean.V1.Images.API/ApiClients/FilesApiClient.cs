using AStar.Clean.V1.Images.API.Config;

namespace AStar.Clean.V1.Images.API.ApiClients;

public class FilesApiClient
{
    private readonly HttpClient httpClient;

    public FilesApiClient(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task<HttpResponseMessage> GetFileListAsync(SearchParameters searchParameters) => await httpClient.GetAsync($"api/files?{searchParameters}");

    public async Task<HttpResponseMessage> GetFileListCountAsync(SearchParameters searchParameters) => await httpClient.GetAsync($"api/filesCount?{searchParameters}");
}
