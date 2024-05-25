using AStar.Clean.V1.BlazorUI.Models;
using Microsoft.AspNetCore.Components;

namespace AStar.Clean.V1.BlazorUI.Components;

public partial class FileDelete
{
    [Parameter]
    public string? Fullname { get; set; }

    [Parameter]
    public SearchParameters? SearchParameters { get; set; }

    [Parameter]
    public EventCallback<string> OnDelete { get; set; }

    [Inject]
    public NavigationManager MyNavigationManager { get; set; } = default!;

    private bool IsEnabled { get; set; } = true;

    private async Task DeleteImageAsync() => await DeleteAsync(deleteHard: false);

    private async Task DeleteImageHardAsync() => await DeleteAsync(deleteHard: true);

    private async Task DeleteAsync(bool deleteHard)
    {
        IsEnabled = false;
        _ = TemporaryHackToOverrideHardDeleteFromListPage(deleteHard);

        var response = await FilesApiClient.DeleteFileAsync(Fullname!);

        var message = !response.IsSuccessStatusCode ? $"Could not delete the file. Please try again.Error Message: {response.ReasonPhrase}" : "Deleted.";

        await OnDelete.InvokeAsync(message);
    }

    private bool TemporaryHackToOverrideHardDeleteFromListPage(bool deleteHard)
    {
        if(MyNavigationManager.Uri.Contains("images/list"))
        { deleteHard = false; }

        return deleteHard;
    }
}
