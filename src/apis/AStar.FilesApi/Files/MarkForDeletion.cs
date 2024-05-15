using Ardalis.ApiEndpoints;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class MarkForDeletion(FilesContext context, ILogger<MarkForDeletion> logger)
            : EndpointBaseSync
                    .WithRequest<string>
                    .WithActionResult
{
    [HttpDelete("mark-for-deletion")]
    [SwaggerOperation(
        Summary = "Mark the specified file for deletion",
        Description = "Mark the specified file for deletion - the file will NOT be deleted, just marked for deletion, please run the separate delete method to actually delete the file.",
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
            specifiedFile.DeletePending = true;
            _ = context.SaveChanges();
        }

        logger.LogDebug("File {FileName} marked for deletion", specifiedFile);

        return NoContent();
    }
}
