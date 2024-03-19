namespace AStar.Clean.V1.BlazorUI.Models;

public class ApiConfiguration
{
    public FilesApiConfiguration FilesApiConfiguration { get; set; } = new();

    public ImagesApiConfiguration ImagesApiConfiguration { get; set; } = new();
}
