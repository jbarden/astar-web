using Ardalis.ApiEndpoints;
using AStar.FilesApi.Config;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class List(FilesContext context, ILogger<List> logger)
            : EndpointBaseSync
                    .WithRequest<SearchParameters>
                    .WithActionResult<IReadOnlyCollection<FileInfoDto>>
{
    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "List the matching files",
        Description = "List the files matching the criteria",
        OperationId = "Files_List",
        Tags = ["Files"])
]
    public override ActionResult<IReadOnlyCollection<FileInfoDto>> Handle([FromQuery] SearchParameters request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        logger.LogDebug("Starting {SearchType} search...{FullParameters}", request.SearchType, request);
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        var files = context.Files.FilterBySearchFolder(request.SearchFolder, request.Recursive);
        var fileList = new List<FileInfoDto>();
        foreach(var file in files.Skip((request.CurrentPage - 1) * request.ItemsPerPage).Take(request.ItemsPerPage))
        {
            fileList.Add(new FileInfoDto(file));
        }

        return Ok(fileList);
    }
}
