using AStar.FilesApi.Client.SDK.FilesApi;
using AStar.FilesApi.Client.SDK.Models;
using AStar.Web.UI.ApiClients.ImagesApi;
using AStar.Web.UI.Shared;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class Dashboard
{
    private const string NotChecked = "Not yet checked...";
    private const string CheckingText = "Checking...please wait...";
    private static readonly string WarningClass = "alert alert-warning";
    private readonly string healthCheckFailure = "alert alert-danger";
    private readonly string healthCheckSuccess = "alert alert-success";
    private readonly string healthCheckWarning = WarningClass;
    private string ImagesApiHealthCheckClass = WarningClass;
    private string FilesApiHealthCheckClass = WarningClass;
    private string FilesApiHealthStatus = NotChecked;
    private string ImagesApiHealthStatus = NotChecked;

    private LoadingIndicator loadingIndicator = new();

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
        SetStatusCheckingDetails();

        var filesApiStatus = await FilesApiClient.GetHealthAsync();
        var imagesApiStatus = await ImagesApiClient.GetHealthAsync();
        FilesApiHealthCheckClass = SetHealthCheckClass(filesApiStatus);
        ImagesApiHealthCheckClass = SetHealthCheckClass(imagesApiStatus);
        FilesApiHealthStatus = filesApiStatus.Status;
        ImagesApiHealthStatus = imagesApiStatus.Status;

        await loadingIndicator.Hide();
    }

    private string SetHealthCheckClass(HealthStatusResponse healthStatusResponse) => healthStatusResponse.Status == "Healthy" ? healthCheckSuccess : healthCheckFailure;

    private void SetStatusCheckingDetails()
    {
        FilesApiHealthCheckClass = healthCheckWarning;
        ImagesApiHealthCheckClass = healthCheckWarning;
        FilesApiHealthStatus = CheckingText;
        ImagesApiHealthStatus = CheckingText;
    }
}
