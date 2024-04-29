using AStar.FilesApi.Config;
using AStar.FilesApi.Controllers;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
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

        var fileCount = 0;
        if(searchParameters.RecursiveSubDirectories)
        {
            fileCount = context.Files.Count(file => file.DirectoryName.StartsWith(searchParameters.SearchFolder));
        }
        else
        {
            fileCount = context.Files.Count(file => file.DirectoryName.Equals(searchParameters.SearchFolder));
        }

        logger.LogDebug("File Count: {FileCount}", fileCount);
        await Task.Delay(1);

        return Ok(fileCount);
    }
}
