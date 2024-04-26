namespace AStar.Web.UI.ApiClients;

public class FilesApiClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<FilesApiClient> logger;

    public FilesApiClient(HttpClient httpClient, ILogger<FilesApiClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<string> GetHealthAsync() => throw new NotImplementedException();
}
