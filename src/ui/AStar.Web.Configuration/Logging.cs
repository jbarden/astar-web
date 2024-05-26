using System.Text.Json;

namespace AStar.Web.Configuration;

public partial class Logging
{
    public Loglevel LogLevel { get; set; } = new();

    public override string ToString() => JsonSerializer.Serialize(this);
}
