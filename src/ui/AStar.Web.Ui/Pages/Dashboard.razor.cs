using AStar.Web.UI.FilesApi;
using AStar.Web.UI.ImagesApi;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class Dashboard
{
    private readonly string healthCheckFailure = "alert alert-danger";
    private readonly string healthCheckSuccess = "alert alert-success";
    private readonly string healthCheckWarning = "alert alert-warning";
    private string ImagesApiHealthCheckClass = "alert alert-warning";
    private string FilesApiHealthCheckClass = "alert alert-warning";

    private LoadingIndicator loadingIndicator = new();

    private string FilesApiHealthStatus = "Not yet checked...";

    private string ImagesApiHealthStatus = "Not yet checked...";

    [Inject]
    private FilesApiClient FilesApiClient { get; set; } = default!;

    [Inject]
    private ImagesApiClient ImagesApiClient { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(!firstRender)
        {
            return;
            
        }
        
        await CheckApiStatuses();
    }

    private async Task CheckApiStatuses()
    {
        await loadingIndicator.Show();
        FilesApiHealthCheckClass = healthCheckWarning;
        ImagesApiHealthCheckClass = healthCheckWarning;
        FilesApiHealthStatus = "Checking...please wait...";
        ImagesApiHealthStatus = "Checking...please wait...";

        var filesApiStatus = await FilesApiClient.GetHealthAsync();
        var imagesApiStatus = await ImagesApiClient.GetHealthAsync();
        FilesApiHealthCheckClass = filesApiStatus.Status == "Healthy" ? healthCheckSuccess : healthCheckFailure;
        ImagesApiHealthCheckClass = imagesApiStatus.Status == "Healthy" ? healthCheckSuccess : healthCheckFailure;
        await Task.Delay(3000); // Do work ...
        FilesApiHealthStatus = filesApiStatus.Status;
        ImagesApiHealthStatus = imagesApiStatus.Status;

        await loadingIndicator.Hide();
    }
}
