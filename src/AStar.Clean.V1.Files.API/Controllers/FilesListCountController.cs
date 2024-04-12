using System.IO.Abstractions;
using AStar.Clean.V1.Files.API.Config;
using AStar.Clean.V1.Files.API.Models;
using AStar.Clean.V1.Files.API.Services;
using AStar.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Files.API.Controllers;

[Route("api/filesCount")]
[ApiController]
public class FilesListCountController(IFileSystem fileSystem, IImageService imageService, FilesContext context, ILogger<FilesControllerBase> logger) : FilesControllerBase(fileSystem, imageService, context, logger)
{
    [HttpGet(Name = "FilesListCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<int> Get([FromQuery] SearchParameters searchParameters)
    {
        var filesList = FileInfoFromContext(searchParameters).ToList();
        filesList = filesList.Where(f => f.FileSize > 0).ToList();

        if(searchParameters.SearchType is SearchType.Images)
        {
            filesList = filesList.Where(f => f.IsImage).ToList();
        }

        Logger.LogInformation("Starting search for {SearchType}", searchParameters.SearchType);
        if(searchParameters.SearchType is not SearchType.Duplicates)
        {
            return Ok(filesList.Count);
        }

        Logger.LogInformation("Starting duplicate search");
        var duplicateFileInfoJbs = DuplicateFileInfoJbs(filesList.Select(f => new FileInfoDto { FullName = Path.Combine(f.DirectoryName, f.FileName), Size = f.FileSize, Name = f.FileName }));
        Logger.LogInformation("Found {DuplicateCount} for {SearchParameters}", duplicateFileInfoJbs.Count,
            searchParameters);

        return Ok(duplicateFileInfoJbs.Count);
    }
}
