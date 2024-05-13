using Ardalis.ApiEndpoints;
using AStar.FilesApi.Config;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class UndoMarkForDeletion(FilesContext context, ILogger<MarkForDeletion> logger)
            : EndpointBaseSync
                    .WithRequest<string>
                    .WithActionResult
{
    [HttpDelete("UndoMarkForDeletion")]
    [SwaggerOperation(
        Summary = "Undo mark the specified file for deletion",
        Description = "Undo the mark the specified file for deletion.",
        OperationId = "Files_MarkForDeletiong",
        Tags = ["Files"])
]
    public override ActionResult Handle(string request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if(request.IsNullOrWhiteSpace() || !request.Contains('\\'))
        {
            return BadRequest("A valid file with path must be specified.");
        }

        var index = request.LastIndexOf('\\');
        var directory = request[..index];
        var fileName = request[++index..];
        var specifiedFile = context.Files.FirstOrDefault(file => file.DirectoryName == directory && file.FileName == fileName);
        if(specifiedFile != null)
        {
            specifiedFile.DeletePending = false;
            _ = context.SaveChanges();
        }

        logger.LogDebug("File {FileName} mark for deletion has been undone", specifiedFile);

        return NoContent();
    }
}
