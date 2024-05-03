using AStar.FilesApi.Config;
using AStar.Infrastructure;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesApi.Files;

[Route("api/[controller]")]
[ApiController]
public class FileDuplicatesCounterController(FilesContext context, ILogger<FileDuplicatesCounterController> logger) : ControllerBase
{
    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiExplorerSettings(GroupName = "FilesJB")]
    public ActionResult<int> GetDuplicates([FromQuery] SearchParameters searchParameters)
    {
        ArgumentNullException.ThrowIfNull(searchParameters);
        if(searchParameters.SearchType != SearchType.Duplicates)
        {
            return BadRequest("Only Duplicate searches are supported by this endpoint, please call the non-duplicate-specific endpoint for any other count.");
        }

        var matchingFiles = GetMatchingFiles(context, searchParameters)
                          .GroupDuplicates();

        logger.LogDebug("Duplicate File Groups Count: {DuplicateFileGroupsCount}", matchingFiles);

        return Ok(matchingFiles);
    }

    private static IEnumerable<FileDetail> GetMatchingFiles(FilesContext context, SearchParameters searchParameters)
        => context.Files
                  .FilterBySearchFolder(searchParameters.SearchFolder, searchParameters.Recursive)
                  .FilterImagesIfApplicable(searchParameters.SearchType.ToString());
}
