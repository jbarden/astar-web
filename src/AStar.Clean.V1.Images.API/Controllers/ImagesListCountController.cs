using AStar.Clean.V1.Images.API.ApiClients;
using AStar.Clean.V1.Images.API.Config;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Images.API.Controllers;

[Route("api/imagesListCount")]
[ApiController]
public class ImagesListCountController : ControllerBase
{
    private readonly FilesApiClient filesApiClient;
    private readonly ILogger<ImagesListController> logger;

    public ImagesListCountController(FilesApiClient filesApiClient, ILogger<ImagesListController> logger)
    {
        this.filesApiClient = filesApiClient;
        this.logger = logger;
    }

    [HttpGet(Name = "ImagesListCount")]
    public async Task<IActionResult> Get([FromQuery] SearchParameters searchParameters)
    {
        if (!searchParameters.CountOnly)
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

        if (!filesResponse.IsSuccessStatusCode)
        {
            return new();
        }

        var fileCountAsString = await filesResponse.Content.ReadAsStringAsync();

        return int.TryParse(fileCountAsString, out var fileCount)
            ? fileCount
            : new();
    }
}
