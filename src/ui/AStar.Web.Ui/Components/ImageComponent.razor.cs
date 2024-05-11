using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Components;

public partial class ImageComponent : ComponentBase
{
    private string imageSource = string.Empty;

    [Parameter]
    public string ImageName { get; set; } = string.Empty;

    [Parameter]
    public string ImageFullName { get; set; } = string.Empty;

    [Parameter]
    public int ImageSize { get; set; } = 150;

    [Parameter]
    public EventCallback<bool> OnDelete { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var image = await ImagesApiClient.GetImageAsync(ImageFullName, ImageSize, true);

        imageSource = await PopulateImageFromStream(image);
    }

    private async Task<string> PopulateImageFromStream(Stream stream)
    {
        var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var b64String = Convert.ToBase64String(ms.ToArray());
        await stream.DisposeAsync();

        if(!b64String.Contains("QUUUUCCiiigAooooA"))
        {
            return $"data:image/png;base64,{b64String}";
        }

        if(OnDelete.HasDelegate)
        {
            await OnDelete.InvokeAsync(true);
        }

        return $"data:image/png;base64,{b64String}";
    }
}
