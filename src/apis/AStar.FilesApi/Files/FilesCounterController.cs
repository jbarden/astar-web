using AStar.FilesApi.Config;
using AStar.FilesApi.Controllers;
using AStar.Infrastructure.Data;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesApi.Files;

[Route("api/[controller]")]
[ApiController]
public class FilesCounterController(FilesContext context, ILogger<FilesControllerBase> logger) : ControllerBase
{
    [HttpGet(Name = "FilesCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<int> Get([FromQuery] SearchParameters searchParameters)
    {
        ArgumentNullException.ThrowIfNull(searchParameters);
        if(searchParameters.SearchType == SearchType.Duplicates)
        {
            return BadRequest("Duplicate searches are not supported by this endpoint, please call the duplicate-specific endpoint.");
        }

        var matchingFiles = GetMatchingFiles(context, searchParameters);

        logger.LogDebug("File Count: {FileCount}", matchingFiles.Count());

        return Ok(matchingFiles.Count());
    }

    [HttpGet(Name = "DuplicatesCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
                  .FilterBySearchFolder(searchParameters.SearchFolder, searchParameters.RecursiveSubDirectories)
                  .FilterImagesIfApplicable(searchParameters.SearchType);
}
