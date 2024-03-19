using System.Drawing;

namespace AStar.Clean.V1.Files.API.Services;

public interface IImageService
{
    Image GetImage(string imagePath);
}
