using System.Drawing;
using AStar.Clean.V1.DomainModel;

namespace AStar.Clean.V1.Files.WorkerService.Services;

public class ImageService : IImageService
{
    public Dimensions GetImage(string imagePath)
    {
        using var image = Image.FromFile(imagePath);

        return new() { Height = image.Height, Width = image.Width };
    }
}
