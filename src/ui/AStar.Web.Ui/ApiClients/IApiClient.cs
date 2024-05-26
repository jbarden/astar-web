using AStar.Web.UI.Shared;

namespace AStar.Web.UI.ApiClients;

public interface IApiClient
{
    public Task<HealthStatusResponse> GetHealthAsync();
}
