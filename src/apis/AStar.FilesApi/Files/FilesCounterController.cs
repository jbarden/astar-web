using AStar.FilesApi.Config;
using AStar.FilesApi.Controllers;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;
using static AStar.Utilities.StringExtensions;

namespace AStar.FilesApi.Files;

[Route("api/[controller]")]
[ApiController]
public class FilesCounterController(FilesContext context, ILogger<FilesControllerBase> logger) : ControllerBase
{
    [HttpGet(Name = "FilesCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<int> Get([FromQuery] SearchParameters searchParameters)
    {
        ArgumentNullException.ThrowIfNull(searchParameters);
        if(searchParameters.SearchFolder.IsNullOrWhiteSpace())
        {
            return Ok(0);
        }

        var fileCountAsQueryable = context.Files as IQueryable<FileDetail>;
        if(searchParameters.SearchType == SearchType.Images)
        {
            fileCountAsQueryable = fileCountAsQueryable.Where(file => file.IsImage);
        }

        fileCountAsQueryable = searchParameters.RecursiveSubDirectories
            ? fileCountAsQueryable.Where(file => file.DirectoryName.StartsWith(searchParameters.SearchFolder))
            : fileCountAsQueryable.Where(file => file.DirectoryName.Equals(searchParameters.SearchFolder));

        if(searchParameters.SearchType == SearchType.Duplicates)
        {
            var duplicatesBySize = fileCountAsQueryable.AsEnumerable()
                .GroupBy(file => FileSize.Create(file.FileSize, file.Height, file.Width),
                    new FileSizeEqualityComparer()).Where(files => files.Count() > 1)
                .ToArray();

            return Ok(duplicatesBySize.Count());
        }

        logger.LogDebug("File Count: {FileCount}", fileCountAsQueryable.Count());

        return Ok(fileCountAsQueryable.Count());
    }
}
