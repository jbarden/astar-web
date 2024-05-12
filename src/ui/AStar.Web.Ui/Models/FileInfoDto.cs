namespace AStar.Web.UI.Models;

public class FileInfoDto
{
    public string Name { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public long Height { get; set; }

    public long Width { get; set; }

    public long Size { get; set; }

    public string SizeForDisplay 
                        => Size / 1024 / 1024 > 0
                                ? (Size / 1024 / 1024).ToString() + "Mb"
                                : (Size / 1024).ToString() + "Kb";

    public string ChecksumHash { get; set; } = string.Empty;

    public DateTime? Created { get; set; }
}
