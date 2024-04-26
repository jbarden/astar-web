namespace AStar.Web.UI.FilesApi;

public class FilesApiConfiguration
{
    public const string SectionLocation = "ApiConfiguration:FilesApiConfiguration";

    public Uri BaseUrl { get; set; } = new("http://not.set.com");
}
