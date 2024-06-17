using System.Text.Json;

namespace AStar.FilesApi.Client.SDK.Models;

/// <summary>
/// The <see href="DuplicateGroup"></see> class.
/// </summary>
public class DuplicateGroup
{
    /// <summary>
    /// Gets or sets the Id of the File Group.
    /// </summary>
    public FileDimensionsWithSize Group { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of <see href="FileInfoDto"></see>.
    /// </summary>
    public IReadOnlyCollection<FileDetail> Files { get; set; } = [];

    /// <summary>
    /// Returns this object in JSON format.
    /// </summary>
    /// <returns>This object serialized as a JSON object.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
