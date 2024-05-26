using System.Text.Json;

namespace AStar.Web.Configuration;

public partial class FilesApiConfiguration
{
    public string BaseUrl { get; set; } = "https://not.set.com";

    public override string ToString() => JsonSerializer.Serialize(this);
}
