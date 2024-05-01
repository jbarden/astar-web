using AStar.FilesApi.Files;
using AStar.FilesApi.Models;
using AStar.Infrastructure.Data;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AStar.FilesApi.Controllers;

[Route("api/files")]
[ApiController]
public class FilesControllerBase(FilesContext context, ILogger<FilesControllerBase> logger) : ControllerBase
{
    protected ILogger<FilesControllerBase> Logger { get; set; } = logger;

    protected IQueryable<FileDetail> FileInfoFromContext(SearchParameters searchParameters)
        => context.Files.Where(f => f.FileSize > 0 && !f.SoftDeleted && searchParameters.Recursive
                                            ? f.DirectoryName.ToUpper().StartsWith(searchParameters.SearchFolder.ToUpper())
                                            : f.DirectoryName.ToUpper().Equals(searchParameters.SearchFolder.ToUpper()));

    protected List<FileInfoDto> DuplicateFileInfoJbs(IEnumerable<FileInfoDto> filesList)
    {
        var duplicatesBySize = filesList.Where(fileInfoDto => fileInfoDto.IsImage())
                .GroupBy(file => FileSize.Create(file.Size, file.Height, file.Width),
                    new FileSizeEqualityComparer()).Where(files => files.Count() > 1)
                .ToArray();

        var duplicates = new List<FileInfoDto>();
        foreach(var fileGroup in duplicatesBySize)
        {
            duplicates.AddRange(fileGroup);
        }

        return duplicates;
    }
}
