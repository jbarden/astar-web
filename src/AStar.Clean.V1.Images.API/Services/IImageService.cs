using System.Drawing;

namespace AStar.Clean.V1.Images.API.Services;

public interface IImageService
{
    Image GetImage(string imagePath);
}