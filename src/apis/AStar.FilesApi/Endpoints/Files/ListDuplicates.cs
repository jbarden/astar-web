﻿using System.Diagnostics;
using Ardalis.ApiEndpoints;
using AStar.FilesApi.Files;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static AStar.Infrastructure.EnumerableExtensions;

namespace AStar.FilesApi.Endpoints.Files;

[Route("api/files")]
public class ListDuplicates(FilesContext context, ILogger<ListDuplicates> logger)
            : EndpointBaseAsync
                    .WithRequest<ListDuplicatesSearchParameters>
                    .WithActionResult<IReadOnlyCollection<DuplicateGroup>>
{
    [HttpGet("list-duplicates")]
    [SwaggerOperation(
        Summary = "List the matching duplicate files",
        Description = "List the duplicate files matching the criteria and group by size and dimensions",
        OperationId = "Files_ListDuplicates",
        Tags = ["Files"])
]
    public override async Task<ActionResult<IReadOnlyCollection<DuplicateGroup>>> HandleAsync([FromQuery] ListDuplicatesSearchParameters request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var stopwatch = Stopwatch.StartNew();
        if(request.SearchFolder.IsNullOrWhiteSpace())
        {
            return BadRequest("A Search folder must be specified.");
        }

        logger.LogDebug("Starting {SearchType} search...{FullParameters}", request.SearchType, request);

        var files = context.Files
                           .GetMatchingFiles(request.SearchFolder, request.Recursive, request.SearchType.ToString(), request.IncludeSoftDeleted, request.IncludeMarkedForDeletion, request.ExcludeViewed, cancellationToken)
                           .OrderFiles(request.SortOrder)
                           .GetDuplicates();

        var fileList = new List<DuplicateGroup>();

        foreach(var fileGroup in files.Skip((request.CurrentPage - 1) * request.ItemsPerPage).Take(request.ItemsPerPage))
        {
            logger.LogDebug("Got the results for the {SearchType} search (Total seconds: {TotalSeconds}) and are about to process them for Page {Page}...{FullParameters}", request.SearchType, stopwatch.Elapsed.Seconds, request.CurrentPage, request);

            var key = fileGroup.Key;
            var fileDtos = new List<FileInfoDto>();

            foreach(var file in fileGroup)
            {
                fileDtos.Add(new FileInfoDto(file));
                file.LastViewed = DateTime.UtcNow;
            }

            fileList.Add(new DuplicateGroup() { Group = new() { FileLength = key.FileLength, Width = key.Width, Height = key.Height }, Files = fileDtos });
        }

        _ = context.SaveChanges();
        await Task.Delay(1, cancellationToken);
        logger.LogDebug("About to return the results for the {SearchType} search (Total seconds: {TotalSeconds})...{FullParameters}", request.SearchType, stopwatch.Elapsed.Seconds, request);

        return Ok(fileList);
    }
}
