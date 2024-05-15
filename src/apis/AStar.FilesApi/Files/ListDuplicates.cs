using Ardalis.ApiEndpoints;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AStar.Infrastructure.EnumerableExtensions;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class ListDuplicates(FilesContext context, ILogger<ListDuplicates> logger)
            : EndpointBaseSync
                    .WithRequest<SearchParameters>
                    .WithActionResult<IReadOnlyCollection<DuplicateGroup>>
{
    [HttpGet("list-duplicates")]
    [SwaggerOperation(
        Summary = "List the matching duplicate files",
        Description = "List the duplicate files matching the criteria and group by size and dimensions",
        OperationId = "Files_List",
        Tags = ["Files"])
]
    public override ActionResult<IReadOnlyCollection<DuplicateGroup>> Handle([FromQuery] SearchParameters request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        logger.LogDebug("Starting {SearchType} search...{FullParameters}", request.SearchType, request);

        var files = context.Files
                           .GetMatchingFiles(request.SearchFolder, request.Recursive, request.SearchType.ToString(), request.IncludeSoftDeleted, request.IncludeMarkedForDeletion)
                           .OrderFiles(request.SortOrder)
                           .GetDuplicates();

        var fileList = new List<DuplicateGroup>();

        foreach(var fileGroup in files.Skip((request.CurrentPage - 1) * request.ItemsPerPage).Take(request.ItemsPerPage))
        {
            var key = fileGroup.Key;
            var fileDtos = new List<FileInfoDto>();

            foreach(var file in fileGroup)
            {
                fileDtos.Add(new FileInfoDto(file));
            }

            fileList.Add(new DuplicateGroup() { Group = new() { FileLength = key.FileLength, Width = key.Width, Height = key.Height }, Files = fileDtos });
        }

        return Ok(fileList);
    }
}
