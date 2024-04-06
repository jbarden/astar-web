using System.IO.Abstractions;
using AStar.Clean.V1.Files.API.Config;
using AStar.Clean.V1.Files.API.Models;
using AStar.Clean.V1.Files.API.Services;
using AStar.Infrastructure.Data;
using AStar.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Files.API.Controllers;

[Route("api/files")]
[ApiController]
public class FilesControllerBase(IFileSystem fileSystem, IImageService imageService, FilesContext context, ILogger<FilesControllerBase> logger) : ControllerBase
{
    protected IFileSystem FileSystem { get; set; } = fileSystem;

    protected IImageService ImageService { get; set; } = imageService;

    protected ILogger<FilesControllerBase> Logger { get; set; } = logger;

    protected IQueryable<FileDetail> FileInfoFromContext(SearchParameters searchParameters)
        => context.Files.Where(f => f.FileSize > 0 && !f.SoftDeleted && searchParameters.RecursiveSubDirectories
                                            ? f.DirectoryName.StartsWith(searchParameters.SearchFolder, StringComparison.CurrentCultureIgnoreCase)
                                            : f.DirectoryName.ToUpper().Equals(searchParameters.SearchFolder.ToUpper()));

    protected List<FileInfoDto> DuplicateFileInfoJbs(IEnumerable<FileInfoDto> filesList)
    {
        try
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
        catch(Exception ex)
        {
            Logger.LogError(ex, "Error: {error}", ex.Message);
            throw;
        }
    }
}
