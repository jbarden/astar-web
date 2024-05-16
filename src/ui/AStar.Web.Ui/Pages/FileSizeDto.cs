namespace AStar.Web.UI.Pages;

public class FileSizeDto
{
    /// <summary>
    /// Gets the file length property.
    /// </summary>
    public long FileLength { get; set; }

    /// <summary>
    /// Gets the file height property.
    /// </summary>
    public long Height { get; set; }

    /// <summary>
    /// Gets the file width property.
    /// </summary>
    public long Width { get; set; }

    public string FileSizeForDisplay
                        => FileLength / 1024 / 1024 > 0
                                ? (FileLength / 1024D / 1024D).ToString("N2") + " Mb"
                                : (FileLength / 1024D).ToString("N2") + " Kb";
}
