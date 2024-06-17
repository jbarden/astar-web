using System.Text.Json;

namespace AStar.FilesApi.Client.SDK.Models;

/// <summary>
/// The <see href="FileDetail"></see> class.
/// </summary>
public class FileDetail
{
    /// <summary>
    /// Gets or sets the Id of the File Detail.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the Name of the file.
    /// </summary>
    public string Name { get; set; } = "Not Set";

    /// <summary>
    /// Gets or sets the Full Name (including the path) of the file.
    /// </summary>
    public string FullName { get; set; } = "Not Set";

    /// <summary>
    /// Gets or sets the Directory Name of the file. This is the same as the <see href="FullName"></see> without the file name.
    /// </summary>
    public string DirectoryName => FullName.Replace(Name, string.Empty);

    /// <summary>
    /// Gets or sets the Height of the image (if applicable).
    /// </summary>
    public long Height { get; set; }

    /// <summary>
    /// Gets or sets the Width of the image (if applicable).
    /// </summary>
    public long Width { get; set; }

    /// <summary>
    /// Gets or sets the size of the file.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Gets the file size, but converted to Mb/Kb for display
    /// </summary>
    public string SizeForDisplay
                        => Size / 1024 / 1024 > 0
                                ? (Size / 1024D / 1024D).ToString("N2") + " Mb"
                                : (Size / 1024D).ToString("N2") + " Kb";

    /// <summary>
    /// Returns this object in JSON format.
    /// </summary>
    /// <returns>This object serialized as a JSON object.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
