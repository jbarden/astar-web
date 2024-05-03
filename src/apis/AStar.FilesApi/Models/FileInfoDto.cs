using AStar.Web.Domain;

namespace AStar.FilesApi.Models;

public class FileInfoDto
{
    public FileInfoDto(FileDetail fileDetail)
    {
        Name = fileDetail.FileName;
        FullName = Path.Combine(fileDetail.DirectoryName, fileDetail.FileName);
        Height = fileDetail.Height;
        Width = fileDetail.Width;
        Size = fileDetail.FileSize;
        Height = fileDetail.Height;
        DetailsLastUpdated = fileDetail.DetailsLastUpdated;
        LastViewed = fileDetail.LastViewed;
    }

    public FileInfoDto()
    {
    }

    public string Name { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public long Height { get; set; }

    public long Width { get; set; }

    public long Size { get; set; }

    /// <summary>
    /// Gets or sets the date the file details were last updated. I know, shocking...
    /// </summary>
    public DateTime? DetailsLastUpdated { get; set; }

    /// <summary>
    /// Gets or sets the date the file wase last viewed. I know, shocking...
    /// </summary>
    public DateTime? LastViewed { get; set; }

    public string Extension
    {
        get
        {
            var extensionIndex = Name.LastIndexOf(".", StringComparison.Ordinal) + 1;
            return Name[extensionIndex..];
        }
    }
}
