using Ardalis.ApiEndpoints;
using AStar.Infrastructure.Data;
using AStar.Utilities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Files;

[Route("api/files")]
public class MarkForMoving(FilesContext context, ILogger<MarkForMoving> logger)
            : EndpointBaseSync
                    .WithRequest<string>
                    .WithActionResult
{
    [HttpPut("mark-for-moving")]
    [SwaggerOperation(
        Summary = "Mark the specified file for moving later",
        Description = "Mark the specified file for moving - the file will NOT be moved, just marked for moving. Please use the applicable page in the portal to actually perform the move.",
        OperationId = "Files_MarkForMoving",
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
            specifiedFile.NeedsToMove = true;
            _ = context.SaveChanges();
        }

        logger.LogDebug("File {FileName} marked for deletion", request);

        return NoContent();
    }
}
