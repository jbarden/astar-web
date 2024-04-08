namespace AStar.Web.Components.Pages;

public partial class Home
{
    public Logger<Index>? Logger { get; set; }

    public string FilesApiHealthStatus { get; set; } = "unknown";

    public string ImagesApiHealthStatus { get; set; } = "unknown";

    public bool Loaded { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var filesApiHealthStatus = await FilesApiClient.GetHealthAsync();
        FilesApiHealthStatus = filesApiHealthStatus.Status;

        var imagesApiHealthStatus = await ImagesApiClient.GetHealthAsync();
        ImagesApiHealthStatus = imagesApiHealthStatus.Status;
        Loaded = true;
    }
}