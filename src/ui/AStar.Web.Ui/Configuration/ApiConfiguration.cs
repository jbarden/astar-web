using AStar.CodeGenerators;

namespace AStar.Web.UI.Configuration;

[GenerateToString]
public partial class ApiConfiguration
{
    public FilesApiConfiguration FilesApiConfiguration { get; set; } = new();

    public ImagesApiConfiguration ImagesApiConfiguration { get; set; } = new();
}
