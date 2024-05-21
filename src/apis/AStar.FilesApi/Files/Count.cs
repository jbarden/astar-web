using Ardalis.ApiEndpoints;
using AStar.FilesApi.Config;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class Count(FilesContext context, ILogger<Count> logger)
            : EndpointBaseAsync
                    .WithRequest<SearchParameters>
                    .WithActionResult<int>
{
    [HttpGet("count")]
    [Produces("application/vnd.astar.file-count+json","application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get the count of files",
        Description = "Get the count of files matching the criteria",
        OperationId = "Files_Count",
        Tags = ["Files"])
]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public override async Task<ActionResult<int>> HandleAsync([FromQuery] SearchParameters request, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest(new { error = "A Search folder must be specified." });
        }

        if(request.SearchType == SearchType.Duplicates)
        {
            return BadRequest("Duplicate searches are not supported by this endpoint, please call the duplicate-specific endpoint.");
        }

        var matchingFilesCount = context.Files
                                        .GetMatchingFiles(request.SearchFolder, request.Recursive, request.SearchType.ToString(), request.IncludeSoftDeleted, request.IncludeMarkedForDeletion, cancellationToken)
                                        .Count();

        logger.LogDebug("File Count: {FileCount}", matchingFilesCount);

        return Ok(matchingFilesCount);
    }
}
