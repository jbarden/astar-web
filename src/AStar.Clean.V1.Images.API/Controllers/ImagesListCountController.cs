using AStar.Clean.V1.Images.API.ApiClients;
using AStar.Clean.V1.Images.API.Config;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Images.API.Controllers;

[Route("api/imagesListCount")]
[ApiController]
public class ImagesListCountController(FilesApiClient filesApiClient, ILogger<ImagesListController> logger) : ControllerBase
{
    [HttpGet(Name = "ImagesListCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> Get([FromQuery] SearchParameters searchParameters)
    {
        if(!searchParameters.CountOnly)
        {
            return BadRequest();
        }

        logger.LogInformation("Starting GetImageList @ {startTime}", DateTime.UtcNow);

        var filesCount = await GetImageList(searchParameters);

        return new OkObjectResult(filesCount);
    }

    private async Task<int> GetImageList(SearchParameters searchParameters)
    {
        var filesResponse = await filesApiClient.GetFileListCountAsync(searchParameters);

        if(!filesResponse.IsSuccessStatusCode)
        {
            return new();
        }

        var fileCountAsString = await filesResponse.Content.ReadAsStringAsync();

        return int.TryParse(fileCountAsString, out var fileCount)
            ? fileCount
            : new();
    }
}
