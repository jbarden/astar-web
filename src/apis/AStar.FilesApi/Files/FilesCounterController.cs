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
    [HttpGet(Name = "FilesListCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Get([FromQuery] SearchParameters searchParameters)
    {
        ArgumentNullException.ThrowIfNull(searchParameters);

        var fileCountAsQueryable = context.Files as IQueryable<FileDetail>;
        if(searchParameters.SearchType == SearchType.Images)
        {
            fileCountAsQueryable = fileCountAsQueryable.Where(file => file.IsImage);
        }

        var fileCount = searchParameters.RecursiveSubDirectories
            ? fileCountAsQueryable.Count(file => file.DirectoryName.StartsWith(searchParameters.SearchFolder))
            : fileCountAsQueryable.Count(file => file.DirectoryName.Equals(searchParameters.SearchFolder));

        logger.LogDebug("File Count: {FileCount}", fileCount);
        await Task.Delay(1);

        return Ok(fileCount);
    }
}
