using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AStar.Web.Components.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public partial class Error(HttpContext context, ILogger<Error> logger) 
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public void OnGet()
    {
        logger.LogInformation("Error");
        RequestId = Activity.Current?.Id ?? context.TraceIdentifier;
    }
}
