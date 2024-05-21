using System.ComponentModel.DataAnnotations;

namespace AStar.Update.Database.WorkerService.Models;

public class ApiConfiguration
{
    public const string SectionLocation = "ApiConfiguration";

    [Required]
    public string[] Directories { get; set; } = [];
}
