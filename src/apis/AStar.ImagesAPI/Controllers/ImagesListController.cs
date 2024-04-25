using AStar.ImagesAPI.ApiClients;
using AStar.ImagesAPI.Config;
using AStar.ImagesAPI.Extensions;
using AStar.ImagesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AStar.ImagesAPI.Controllers;

[Route("api/imagesList")]
[ApiController]
public class ImagesListController(FilesApiClient filesApiClient) : ControllerBase
{
    [HttpGet(Name = "ImagesList")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async IAsyncEnumerable<FileInfoDto> Get([FromQuery] SearchParameters searchParameters)
    {
        CheckParameters(searchParameters);

        await foreach(var p in PerformImageGetAsync(searchParameters))
        {
            yield return p;
        }
    }

    private static void CheckParameters(SearchParameters searchParameters)
    {
        if(searchParameters.CountOnly)
        {
            throw new ArgumentException("Controller cannot return the count only");
        }

        if(!searchParameters.SearchType.Equals(SearchType.Images))
        {
            throw new ArgumentException($"Unsupported file type - {searchParameters.SearchType}.");
        }
    }

    private async IAsyncEnumerable<FileInfoDto> PerformImageGetAsync(SearchParameters searchParameters)
    {
        var fileList = await GetImageList(searchParameters);

        foreach(var fileInfo in fileList)
        {
            yield return fileInfo;
        }
    }

    private async Task<List<FileInfoDto>> GetImageList(SearchParameters searchParameters)
    {
        var filesResponse = await filesApiClient.GetFileListAsync(searchParameters);

        if(!filesResponse.IsSuccessStatusCode)
        {
            return [];
        }

        var files = await filesResponse.Content.ReadFromJsonAsync<IList<FileInfoDto>>();

        return files != null
            ? files.Where(filename => filename.IsImage()).ToList()
            : [];
    }
}
