using System.Reflection;
using Ardalis.ApiEndpoints;
using AStar.FilesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AStar.FilesApi.Endpoints.Root;

[Route("/")]
public class Get : EndpointBaseAsync
                        .WithoutRequest
                        .WithActionResult<IEnumerable<LinkDto>>
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Root Document",
        Description = "List all of the available root methods",
        OperationId = "Root_List",
        Tags = ["Root"])
]
    public override async Task<ActionResult<IEnumerable<LinkDto>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var links = await GetLinks(cancellationToken);

        return Ok(links);
    }

    private async Task<List<LinkDto>> GetLinks(CancellationToken cancellationToken = default)
    {
        var links = new List<LinkDto>();
        await Task.Delay(1, cancellationToken);
        if(cancellationToken.IsCancellationRequested)
        {
            return links;
        }

        IEnumerable<Type> derivedTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t=> t?.Namespace?.Contains("Endpoints")==true);

        foreach(var type in derivedTypes)
        {
            var name = type?.ReflectedType?.Name;
            if(name is not null)
            {
                links.Add(new LinkDto() { Name = name });
            }
        }

        return links;
    }
}
