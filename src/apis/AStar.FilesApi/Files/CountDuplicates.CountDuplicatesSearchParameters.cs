using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using AStar.FilesApi.Config;
using AStar.Web.Domain;

namespace AStar.FilesApi.Files;

public class CountDuplicatesSearchParameters
{
    [Required]
    public string SearchFolder { get; set; } = string.Empty;

    public bool Recursive { get; set; } = true;

    public bool IncludeSoftDeleted { get; set; }

    public bool IncludeMarkedForDeletion { get; set; }

    public string SearchText { get; set; } = string.Empty;

    internal SearchType SearchType { get; } = SearchType.Duplicates;

    public override string ToString() => JsonSerializer.Serialize(this);
}
