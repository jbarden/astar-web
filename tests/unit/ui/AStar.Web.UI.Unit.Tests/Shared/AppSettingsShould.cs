using System.Text.Json;
using AStar.Utilities;
using AStar.Web.UI.Configuration;

namespace AStar.Web.UI.Shared;

public class AppSettingsShould
{
    [Fact]
    public void ContainTheEpectedValueForPaginationPages()
    {
        var appSettings = AppSettingsHelper.MockApplicationSettings.FromJson< ApplicationSettings> (new(JsonSerializerDefaults.Web));

        appSettings.ApplicationConfiguration.PaginationPageDefaultPreAndPostCount.Should().Be(5);
    }
}
