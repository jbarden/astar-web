using System.ComponentModel.DataAnnotations;
using AStar.Update.Database.WorkerService.ApiClients.FilesApi;

namespace AStar.Update.Database.WorkerService.Models;

public class ApiConfiguration
{
    public const string SectionLocation = "ApiConfiguration";

    [Required]
    public string[] Directories { get; set; } = [];

    [Required]
    public FilesApiConfiguration FilesApiConfiguration { get; set; } = new();
}
