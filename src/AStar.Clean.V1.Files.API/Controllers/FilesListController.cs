using System.IO.Abstractions;
using AStar.Clean.V1.Files.API.Config;
using AStar.Clean.V1.Files.API.Models;
using AStar.Clean.V1.Files.API.Services;
using AStar.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Files.API.Controllers;

[Route("api/files")]
[ApiController]
public class FilesListController(IFileSystem fileSystem, IImageService imageService, FilesContext context, ILogger<FilesControllerBase> logger)
    : FilesControllerBase(fileSystem, imageService, context, logger)
{
    [HttpGet(Name = "FilesList")]
    public ActionResult<IAsyncEnumerable<FileInfoDto>> Get([FromQuery] SearchParameters searchParameters)
    {
        Logger.LogInformation("Starting search for {searchType}", searchParameters.SearchType);
        var filesList = FileInfoFromContext(searchParameters).ToList();
        if(searchParameters.SearchType is SearchType.Images)
        {
            filesList = filesList.Where(f => f.IsImage2).ToList();
        }

        var fileInfos = filesList.Select(f => new FileInfoDto { FullName = Path.Combine(f.DirectoryName, f.FileName), Size = f.FileSize, Name = f.FileName });
        var skip = searchParameters.ItemsPerPage * (searchParameters.CurrentPage - 1);
        if(searchParameters.SearchType is SearchType.Duplicates)
        {
            Logger.LogInformation("Starting duplicate search");
            fileInfos = DuplicateFileInfoJbs(fileInfos);
            Logger.LogInformation("Found {duplicateCount} duplicates for {searchParameters}", fileInfos.Count(), searchParameters);
        }

        Logger.LogInformation("Setting Sort Order");
        fileInfos = searchParameters.SortOrder switch
        {
            SortOrder.NameAscending => fileInfos.OrderBy(file => file.FullName),
            SortOrder.SizeDescending => fileInfos.OrderByDescending(file => file.Size),
            SortOrder.SizeAscending => fileInfos.OrderBy(file => file.Size),
            SortOrder.NameDescending => fileInfos.OrderByDescending(file => file.FullName),
            _ => throw new ArgumentOutOfRangeException(nameof(searchParameters)),
        };

        fileInfos = fileInfos.Skip(skip).Take(searchParameters.ItemsPerPage).ToList();

        return Ok(fileInfos);
    }
}
