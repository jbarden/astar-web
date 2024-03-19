namespace AStar.Clean.V1.Files.WorkerService.Extensions;

public static class StringExtensions
{
    public static bool IsImage(this string? fileInfo) => fileInfo.EndsWith(".JPEG", StringComparison.OrdinalIgnoreCase)
                                                         || fileInfo.EndsWith(".JPG",
                                                             StringComparison.OrdinalIgnoreCase)
                                                         || fileInfo.EndsWith(".GIF",
                                                             StringComparison.OrdinalIgnoreCase)
                                                         || fileInfo.EndsWith(".BMP",
                                                             StringComparison.OrdinalIgnoreCase)
                                                         || fileInfo.EndsWith(".JIF",
                                                             StringComparison.OrdinalIgnoreCase);
}
