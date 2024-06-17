using System.Text.Json;

namespace AStar.FilesApi.Client.SDK.Models;

/// <summary>
/// The <see href="HealthStatusResponse"></see> class.
/// </summary>
public class HealthStatusResponse
{
    /// <summary>
    /// Gets or sets the Status for the Health Check.
    /// </summary>
    public string Status { get; set; } = "Unknown";

    /// <summary>
    /// Returns this object in JSON format.
    /// </summary>
    /// <returns>This object serialized as a JSON object.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
