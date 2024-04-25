namespace AStar.Clean.V1.BlazorUI.Pages;

public partial class Index
{
    public Logger<Index>? Logger { get; set; }

    public string FilesApiHealthStatus { get; set; } = "unknown";

    public string ImagesApiHealthStatus { get; set; } = "unknown";

    public bool Loaded { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        var filesApiHealthStatus = await FilesApiClient.GetHealthAsync();
        FilesApiHealthStatus = filesApiHealthStatus.Status;

        var imagesApiHealthStatus = await ImagesApiClient.GetHealthAsync();
        ImagesApiHealthStatus = imagesApiHealthStatus.Status;
        Loaded = true;
    }
}
