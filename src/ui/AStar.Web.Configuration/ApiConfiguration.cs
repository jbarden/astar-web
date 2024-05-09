namespace AStar.Web.UI.Configuration;

public partial class ApiConfiguration
{
    public FilesApiConfiguration FilesApiConfiguration { get; set; } = new();

    public ImagesApiConfiguration ImagesApiConfiguration { get; set; } = new();
}
