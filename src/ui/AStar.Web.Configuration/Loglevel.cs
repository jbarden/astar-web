using System.Text.Json;

namespace AStar.Web.Configuration;

public partial class Loglevel
{
    public string Default { get; set; } = "Information";

    public string MicrosoftAspNetCore { get; set; } = "Warning";

    public override string ToString() => JsonSerializer.Serialize(this);
}
