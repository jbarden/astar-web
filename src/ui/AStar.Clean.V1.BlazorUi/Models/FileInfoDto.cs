namespace AStar.Clean.V1.BlazorUI.Models;

public class FileInfoDto
{
    public string Name { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public long Height { get; set; }

    public long Width { get; set; }

    public long Size { get; set; }

    public string ChecksumHash { get; set; } = string.Empty;

    public DateTime? Created { get; set; }
}
