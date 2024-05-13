using Ardalis.ApiEndpoints;
using AStar.FilesApi.Config;
using AStar.Infrastructure;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class DuplicatesCount(FilesContext context, ILogger<DuplicatesCount> logger)
                    : EndpointBaseSync
                            .WithRequest<SearchParameters>
                            .WithActionResult<int>
{
    [HttpGet("duplicatescount")]
    [SwaggerOperation(
        Summary = "Get duplicate files count",
        Description = "Get the count of duplicate files matching the criteria",
        OperationId = "Duplicates_Count",
        Tags = ["Files"])
]
    public override ActionResult<int> Handle([FromQuery] SearchParameters request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        if(request.SearchType != SearchType.Duplicates)
        {
            return BadRequest("Only Duplicate counts are supported by this endpoint, please call the non-duplicate-specific endpoint.");
        }

        var matchingFiles = context.Files
                                   .GetMatchingFiles(request.SearchFolder, request.Recursive, request.SearchType.ToString(), request.IncludeSoftDeleted, request.IncludeMarkedForDeletion)
                                   .GroupDuplicates();

        logger.LogDebug("Duplicate File Count: {FileCount}", matchingFiles);

        return Ok(matchingFiles);
    }
}
