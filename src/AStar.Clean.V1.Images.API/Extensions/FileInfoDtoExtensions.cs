using AStar.Clean.V1.Images.API.Models;

namespace AStar.Clean.V1.Images.API.Extensions;

public static class FileInfoDtoExtensions
{
    public static bool IsImage(this FileInfoDto fileInfo) => fileInfo.Extension.ToUpperInvariant() is ".JPEG"
        or ".JPG"
        or ".PNG"
        or ".GIF"
        or ".BMP";
}
