using Microsoft.AspNetCore.Components;

namespace AStar.Web.Components.Pages.Images;

public partial class ImageThumbnailWithDetails
{
    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public string FullName { get; set; } = string.Empty;

    [Parameter]
    public int MaximumSizeOfThumbnail { get; set; }

    [Parameter]
    public long Size { get; set; }

    public long Width { get; set; }

    public long Height { get; set; }

    public bool Deleted { get; set; }

    private string MessageClass { get; set; } = string.Empty;

    private string Message { get; set; } = string.Empty;

    public string DisplayImageSize()
    {
        if(Size < 1_000_000)
        {
            return $"{Size / 1.0 / 1024:N2} Kb";
        }

        var size = Convert.ToDecimal(Size / 1.0 / 1024 / 1024);
        return $"{size:N2} Mb";
    }

    public void OnDeleted(string message)
    {
        Message = message;
        MessageClass = message.Equals("Deleted.", StringComparison.OrdinalIgnoreCase)
            ? "alert alert-success"
            : "alert alert-danger";
        Deleted = true;
    }

    protected override async Task OnInitializedAsync()
    {
        var details = await ImagesApiClient.GetImageDetailsAsync(FullName);
        if(details is not null)
        {
            Height = details.Height;
            Width = details.Width;
        }
    }

    private void FileDeleted() => OnDeleted("File not found");
}
