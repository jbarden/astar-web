using System.ComponentModel.DataAnnotations;
using AStar.CodeGenerators;
using AStar.FilesApi.Config;
using AStar.FilesApi.Models;

namespace AStar.FilesApi.Files;

[GenerateToString]
public partial class SearchParameters
{
    [Required]
    public string SearchFolder { get; set; } = string.Empty;

    public SearchType SearchType { get; set; }

    public bool Recursive { get; set; } = true;

    [Required]
    public int CurrentPage { get; set; } = 1;

    [Required]
    public int ItemsPerPage { get; set; } = 10;

    [Range(50, 750, ErrorMessage = "Please specify a thumbnail size between 50 and 750 pixels.")]
    public int MaximumSizeOfThumbnail { get; set; } = 150;

    [Range(50, 999999, ErrorMessage = "Please specify an image size between 500 and 999999 (NOT recommended!) pixels.")]
    public int MaximumSizeOfImage { get; set; } = 1500;

    [Required]
    public SortOrder SortOrder { get; set; } = SortOrder.SizeDescending;

    public string SearchText { get; set; } = string.Empty;
}
