using Ardalis.ApiEndpoints;
using AStar.FilesApi.Config;
using AStar.Infrastructure;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class Count(FilesContext context, ILogger<Count> logger)
            : EndpointBaseSync
                    .WithRequest<SearchParameters>
                    .WithActionResult<int>
{
    [HttpGet("count")]
    [SwaggerOperation(
        Summary = "Get the count of files",
        Description = "Get the count of files matching the criteria",
        OperationId = "Files_Count",
        Tags = new[] { "Files" })
]
    public override ActionResult<int> Handle([FromQuery] SearchParameters request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        if(request.SearchType == SearchType.Duplicates)
        {
            return BadRequest("Duplicate searches are not supported by this endpoint, please call the duplicate-specific endpoint.");
        }

        var matchingFilesCount = context.Files
                                        .GetMatchingFiles(request.SearchFolder, request.Recursive, request.SearchType.ToString()).Count();

        logger.LogDebug("File Count: {FileCount}", matchingFilesCount);

        return Ok(matchingFilesCount);
    }
}
