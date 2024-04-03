using System.IO.Abstractions;
using AStar.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Files.API.Controllers;

[Route("api/files")]
[ApiController]
public class DeleteController : ControllerBase
{
    private readonly IFileSystem fileSystem;
    private readonly FilesContext context;

    public DeleteController(IFileSystem fileSystem, FilesContext context)
    {
        this.fileSystem = fileSystem;
        this.context = context;
    }

    [HttpDelete(Name = "Delete")]
    public IActionResult Delete([FromQuery] string filePath, bool hardDelete)
    {
        var filePathUpdated = filePath.Replace("__", @"\");

        if(fileSystem.File.Exists(filePathUpdated))
        {
            DeleteFile(filePathUpdated);
        }

        var lastIndex = filePath.LastIndexOf(@"\", StringComparison.Ordinal);
        var path = filePath[..lastIndex];
        var fileName = filePath[(lastIndex + 1)..];
        var existing = context.Files.FirstOrDefault(f => f.DirectoryName == path && f.FileName == fileName);
        if(existing == null)
        {
            return NoContent();
        }

        if(hardDelete)
        {
            _ = context.Files.Remove(existing);
        }
        else
        {
            existing.FileSize = 0;
            existing.DetailsLastUpdated = DateTime.Now;
            existing.SoftDeleted = true;
        }

        _ = context.SaveChanges();

        return NoContent();
    }

    private void DeleteFile(string filePathUpdated)
    {
        try
        {
            fileSystem.File.Delete(filePathUpdated);
        }
        catch
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            try
            {
                fileSystem.File.Delete(filePathUpdated);
            }
            catch
            {
            }
        }
    }
}
