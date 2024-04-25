using AStar.ImagesAPI.ApiClients;
using AStar.ImagesAPI.Config;
using Microsoft.AspNetCore.Mvc;

namespace AStar.ImagesAPI.Controllers;

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

        logger.LogInformation("Starting GetImageList @ {StartTime}", DateTime.UtcNow);

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
