using AStar.Clean.V1.Images.API.ApiClients;
using AStar.Clean.V1.Images.API.Config;
using AStar.Clean.V1.Images.API.Extensions;
using AStar.Clean.V1.Images.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AStar.Clean.V1.Images.API.Controllers;

[Route("api/imagesList")]
[ApiController]
public class ImagesListController : ControllerBase
{
    private readonly FilesApiClient filesApiClient;

    public ImagesListController(FilesApiClient filesApiClient) => this.filesApiClient = filesApiClient;

    [HttpGet(Name = "ImagesList")]
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
