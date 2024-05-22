using Ardalis.ApiEndpoints;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AStar.Infrastructure.EnumerableExtensions;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class List(FilesContext context, ILogger<List> logger)
            : EndpointBaseSync
                    .WithRequest<SearchParameters>
                    .WithActionResult<IReadOnlyCollection<FileInfoDto>>
{
    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List the matching files",
        Description = "List the files matching the criteria",
        OperationId = "Files_List",
        Tags = ["Files"])
]
    public override ActionResult<IReadOnlyCollection<FileInfoDto>> Handle([FromQuery] SearchParameters request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        logger.LogDebug("Starting {SearchType} search...{FullParameters}", request.SearchType, request);

        var files = context.Files
                           .GetMatchingFiles(request.SearchFolder, request.Recursive, request.SearchType.ToString(), request.IncludeSoftDeleted, request.IncludeMarkedForDeletion, CancellationToken.None)
                           .OrderFiles(request.SortOrder);

        var fileList = new List<FileInfoDto>();

        foreach(var file in files.Skip((request.CurrentPage - 1) * request.ItemsPerPage).Take(request.ItemsPerPage))
        {
            fileList.Add(new FileInfoDto(file));
            file.LastViewed = DateTime.UtcNow;
        }
        
        _ = context.SaveChanges();

        return Ok(fileList);
    }
}
