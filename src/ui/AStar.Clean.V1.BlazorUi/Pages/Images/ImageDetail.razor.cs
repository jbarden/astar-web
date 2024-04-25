using Microsoft.AspNetCore.Components;

namespace AStar.Clean.V1.BlazorUI.Pages.Images;

public partial class ImageDetails
{
    [Parameter]
    public string? Fullname { get; set; }

    public string? Image { get; set; }

    private string? Name { get; set; }

    private long? Width { get; set; }

    private long? Height { get; set; }

    private long? Size { get; set; }

    private DateTime? CreatedDate { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if(Fullname is not null)
        {
            Fullname = Fullname?.Replace("__", @"\");
            Image = await GetImage(Fullname!);
            var details = await ImagesApiClient.GetImageDetailsAsync(Fullname!);
            CreatedDate = details?.Created;
            Width = details?.Width;
            Height = details?.Height;
            Size = details?.Size;
            Name = details?.Name;
        }
    }

    private static string PopulateImageFromStream(Stream stream)
    {
        var ms = new MemoryStream();
        stream.CopyTo(ms);
        var b64String = Convert.ToBase64String(ms.ToArray());

        return $"data:image/png;base64,{b64String}";
    }

    // Need to get the actual size we want to set
    private async Task<Stream> GetImageStream(string imagePath) => await ImagesApiClient.GetImageAsync(imagePath, 1450);

    private async Task<string> GetImage(string imagePath) => PopulateImageFromStream(await GetImageStream(imagePath));
}
