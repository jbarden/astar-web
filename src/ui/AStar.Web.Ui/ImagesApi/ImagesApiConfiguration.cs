using System.ComponentModel.DataAnnotations;

namespace AStar.Web.UI.ImagesApi;

public class ImagesApiConfiguration
{
    public const string SectionLocation = "ApiConfiguration:ImagesApiConfiguration";

    [Required]
    public Uri BaseUrl { get; set; } = new("http://not.set.com");
}
