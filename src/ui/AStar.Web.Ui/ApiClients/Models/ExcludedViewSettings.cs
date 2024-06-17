using System.Text.Json;

namespace AStar.FilesApi.Client.SDK.Models;

/// <summary>
/// The <see href="ExcludedViewSettings"></see> class.
/// </summary>
public class ExcludedViewSettings
{
    /// <summary>
    /// Gets or sets the excluded viewed items period (in days) for the search.
    /// </summary>
    public int ExcludeViewedPeriodInDays { get; set; } = 7;

    /// <summary>
    /// Gets or sets the Exclude Viewed flag. The time period is configurable via the <see href="ExcludeViewedPeriodInDays"></see> property.
    /// </summary>
    public bool ExcludeViewed { get; set; }

    /// <summary>
    /// Returns this object in JSON format.
    /// </summary>
    /// <returns>This object serialized as a JSON object.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
