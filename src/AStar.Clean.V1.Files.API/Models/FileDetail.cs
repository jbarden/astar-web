namespace AStar.Clean.V1.Files.API.Models;

public class FileDetail
{
    public string FileName { get; set; } = string.Empty;
    public bool IsImage { get; set; }
    public DateTime? DetailsLastUpdated { get; set; }
    public DateTime? LastViewed { get; set; }
    public string DirectoryName { get; set; } = string.Empty;
    public int Height { get; set; }
    public int Width { get; set; }
    public long FileSize { get; set; }
    public bool SoftDeleted { get; set; }
}