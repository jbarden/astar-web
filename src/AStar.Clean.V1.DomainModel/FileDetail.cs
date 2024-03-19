using System.Text;

namespace AStar.Clean.V1.DomainModel;

public class FileDetail
{
    public string FullName => Path.Combine(DirectoryName, FileName);
    public bool IsImage => FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                           || FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                           || FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                           || FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                           || FileName.EndsWith(".jif", StringComparison.OrdinalIgnoreCase)
                           || FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase);
    public string FileName { get; set; } = string.Empty;
    public DateTime? DetailsLastUpdated { get; set; }
    public DateTime? LastViewed { get; set; }
    public string DirectoryName { get; set; } = string.Empty;
    public int Height { get; set; }
    public int Width { get; set; }
    public long FileSize { get; set; }
    public bool SoftDeleted { get; set; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"FileName={FileName}&");
        stringBuilder.Append($"DirectoryName={DirectoryName}&");
        stringBuilder.Append($"IsImage={IsImage}&");
        stringBuilder.Append($"DetailsLastUpdated={DetailsLastUpdated}&");
        stringBuilder.Append($"LastViewed={LastViewed}&");
        stringBuilder.Append($"Height={Height}&");
        stringBuilder.Append($"Width={Width}&");
        stringBuilder.Append($"FileSize={FileSize}&");
        stringBuilder.Append($"SoftDeleted={SoftDeleted}");

        return stringBuilder.ToString();
    }
}
