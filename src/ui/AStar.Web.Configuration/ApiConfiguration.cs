using System.Text.Json;

namespace AStar.Web.Configuration;

public partial class ApiConfiguration
{
    public FilesApiConfiguration FilesApiConfiguration { get; set; } = new();

    public ImagesApiConfiguration ImagesApiConfiguration { get; set; } = new();

    public override string ToString() => JsonSerializer.Serialize(this);
}
