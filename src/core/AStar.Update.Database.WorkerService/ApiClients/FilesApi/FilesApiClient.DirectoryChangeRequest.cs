using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AStar.Update.Database.WorkerService.ApiClients.FilesApi;

public class DirectoryChangeRequest
{
    [Required]
    public string OldDirectoryName { get; set; } = string.Empty;

    [Required]
    public string NewDirectoryName { get; set; } = string.Empty;

    [Required]
    public string FileName { get; set; } = string.Empty;

    public override string ToString() => JsonSerializer.Serialize(this);
}
