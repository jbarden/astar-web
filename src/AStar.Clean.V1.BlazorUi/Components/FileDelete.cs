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

    private async Task DeleteImageAsync()
    {
        var response = await FilesApiClient.DeleteFileAsync(Fullname!, false);

        var message = !response.IsSuccessStatusCode ? $"Could not delete the file. Please try again.Error Message: {response.ReasonPhrase}" : "Deleted.";

        await OnDelete.InvokeAsync(message);
    }

    private async Task DeleteImageHardAsync()
    {
        var response = await FilesApiClient.DeleteFileAsync(Fullname!, true);

        var message = !response.IsSuccessStatusCode ? $"Could not delete the file. Please try again.Error Message: {response.ReasonPhrase}" : "Deleted.";

        await OnDelete.InvokeAsync(message);
    }
}
