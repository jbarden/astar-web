using System.ComponentModel.DataAnnotations;

namespace AStar.Update.Database.WorkerService.Models;

public class DirectoryChanges
{
    public const string SectionLocation = "DirectoryChanges";

    [Required]
    public Directory[] Directories { get; set; } = [];
}
