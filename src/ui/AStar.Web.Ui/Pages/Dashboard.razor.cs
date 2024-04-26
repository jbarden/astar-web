using Blazorise.LoadingIndicator;

namespace AStar.Web.UI.Pages;

public partial class Dashboard
{
    private LoadingIndicator loadingIndicator = new();

    protected override async Task OnAfterRenderAsync(bool firstRender) => await CheckApiStatuses();

    private async Task CheckApiStatuses()
    {
        await loadingIndicator.Show();

        await Task.Delay(3000); // Do work ...

        await loadingIndicator.Hide();
    }
}
