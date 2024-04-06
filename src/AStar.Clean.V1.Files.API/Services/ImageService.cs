using System.Drawing;
using System.IO.Abstractions;
using System.Runtime.Versioning;

namespace AStar.Clean.V1.Files.API.Services;

public class ImageService(IFileSystem fileSystem) : IImageService
{
    private readonly IFileSystem fileSystem = fileSystem;

    [SupportedOSPlatform("windows")]
    public Image GetImage(string imagePath) => Image.FromFile(fileSystem.File.Exists(imagePath) ? imagePath : @"d:\wallhaven\wallhaven-l3eze2.jpg");
}
