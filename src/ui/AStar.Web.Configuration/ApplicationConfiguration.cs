using System.Text.Json;

namespace AStar.Web.UI.Configuration;

public partial class ApplicationConfiguration
{
    public int PaginationPageDefaultPreAndPostCount { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}
