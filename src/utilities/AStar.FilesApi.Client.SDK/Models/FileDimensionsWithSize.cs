using System.Text.Json;

namespace AStar.FilesApi.Client.SDK.Models;

/// <summary>
/// The <see href="FileDimensionsWithSize"></see> class.
/// </summary>
public class FileDimensionsWithSize
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

    /// <summary>
    /// Gets the file size, but converted to Mb/Kb for display
    /// </summary>
    public string FileSizeForDisplay
                        => FileLength / 1024 / 1024 > 0
                                ? (FileLength / 1024D / 1024D).ToString("N2") + " Mb"
                                : (FileLength / 1024D).ToString("N2") + " Kb";

    /// <summary>
    /// Returns this object in JSON format.
    /// </summary>
    /// <returns>This object serialized as a JSON object.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
