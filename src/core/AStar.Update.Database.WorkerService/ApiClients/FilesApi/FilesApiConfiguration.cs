using System.ComponentModel.DataAnnotations;

namespace AStar.Update.Database.WorkerService.ApiClients.FilesApi;

public class FilesApiConfiguration
{
    public const string SectionLocation = "ApiConfiguration:FilesApiConfiguration";

    [Required]
    public Uri BaseUrl { get; set; } = new("http://not.set.com");
}
