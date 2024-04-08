using AStar.Web.ApiClients;
using AStar.Web.Models;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.Components;

public partial class FileDelete
{
    [Parameter]
    public string? Fullname { get; set; }

    [Parameter]
    public SearchParameters? SearchParameters { get; set; }

    [Parameter]
    public EventCallback<string> OnDelete { get; set; }

    private bool IsEnabled { get; set; } = true;

    private async Task DeleteImageAsync() => await DeleteAsync(deleteHard: false);

    private async Task DeleteImageHardAsync() => await DeleteAsync(deleteHard: true);

    private async Task DeleteAsync(bool deleteHard)
    {
        IsEnabled = false;
        var response = await FilesApiClient.DeleteFileAsync(Fullname!, deleteHard);

        var message = !response.IsSuccessStatusCode ? $"Could not delete the file. Please try again.Error Message: {response.ReasonPhrase}" : "Deleted.";

        await OnDelete.InvokeAsync(message);
    }
}
