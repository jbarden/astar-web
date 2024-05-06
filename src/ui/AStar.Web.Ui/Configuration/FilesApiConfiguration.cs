using AStar.CodeGenerators;

namespace AStar.Web.UI.Configuration;

[GenerateToString]
public partial class FilesApiConfiguration
{
    public string BaseUrl { get; set; } = "https://not.set.com";
}
