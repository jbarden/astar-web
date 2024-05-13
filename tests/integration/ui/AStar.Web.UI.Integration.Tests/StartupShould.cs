namespace AStar.Web.UI.Integration;

public class StartupShould : IClassFixture<CustomWebApplicationFactory>
{
    public StartupShould(CustomWebApplicationFactory factory) => Factory = factory;

    public CustomWebApplicationFactory Factory { get; }

    [Fact]
    public async Task LoadTheExpectedInitialPage()
    {
        var client = Factory.CreateClient();

        var response = await client.GetAsync("/apps/todo");

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("<title>AStar Web</title>");
        System.IO.File.WriteAllText("c:\\logs\\astar.content-logs.txt", content);
    }
}
