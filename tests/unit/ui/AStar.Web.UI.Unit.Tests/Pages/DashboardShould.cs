using AStar.Web.UI.FilesApi;
using AStar.Web.UI.ImagesApi;
using AStar.Web.UI.Pages;
using AStar.Web.UI.Shared;
using AStar.Web.UI.Unit.Tests.MockMessageHandlers;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Tests.bUnit;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RichardSzalay.MockHttp;

namespace AStar.Web.UI.Unit.Tests.Pages;

public class DashboardShould : TestContext
{
    public DashboardShould()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        Services.AddMockHttpClient();
        Services.AddScoped<FilesApiClient>();
        Services.AddScoped<ImagesApiClient>();
        Services
            .AddBlazoriseTests()
            .AddBootstrap5Providers()
            .AddEmptyIconProvider();
        JSInterop.AddBlazoriseButton();
        Services.AddBlazorise().Replace(ServiceDescriptor.Transient<IComponentActivator, ComponentActivator>());
    }

    [Fact]
    public void LoadTheDefaultHTMLPriorToResultsBeingDisplayed()
    {
        var cut = RenderComponent<Dashboard>();

        var filesApiStatus = cut.Find("#FilesApiHealthStatus");

        filesApiStatus.MarkupMatches(@"<p  id=""FilesApiHealthStatus"" >
  <br>
  <medium class=""alert alert-warning"">
    Files Api Health Status: Checking...please wait...</medium>
</p>");

        var imagesApiStatus = cut.Find("#ImagesApiHealthStatus");

        imagesApiStatus.MarkupMatches(@"<p  id=""ImagesApiHealthStatus"" >
  <br>
  <medium class=""alert alert-warning"">
    Images Api Health Status: Checking...please wait...</medium>
</p>");
    }

    [Fact]
    public void DisplayTheSuccessfulApiStatusWhenApplicablle()
    {
        var mock = Services.AddMockHttpClient();
        mock.When("/health/live").RespondJson(new HealthStatusResponse() { Status = "Healthy" });

        var cut = RenderComponent<Dashboard>();

        var filesApiStatus = cut.Find("#FilesApiHealthStatus");

        cut.WaitForAssertion(() => filesApiStatus.MarkupMatches(@"<p  id=""FilesApiHealthStatus"" >
  <br>
  <medium class=""alert alert-success"">
    Files Api Health Status: Healthy</medium>
</p>"), TimeSpan.FromSeconds(2));

        var imagesApiStatus = cut.Find("#ImagesApiHealthStatus");

        cut.WaitForAssertion(() => imagesApiStatus.MarkupMatches(@"<p  id=""ImagesApiHealthStatus"" >
  <br>
  <medium class=""alert alert-success"">
    Images Api Health Status: Healthy</medium>
</p>"), TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void DisplayTheFailedApiStatusWhenApplicablle()
    {
        var mock = Services.AddMockHttpClient();
        mock.When("/health/live").RespondJson(new HealthStatusResponse() { Status = "Does.Not.Matter" });

        var cut = RenderComponent<Dashboard>();

        var filesApiStatus = cut.Find("#FilesApiHealthStatus");

        cut.WaitForAssertion(() => filesApiStatus.MarkupMatches(@"<p  id=""FilesApiHealthStatus"" >
      <br>
      <medium class=""alert alert-danger"">
        Files Api Health Status: Does.Not.Matter</medium>
    </p>"), TimeSpan.FromSeconds(2));

        var imagesApiStatus = cut.Find("#ImagesApiHealthStatus");

        cut.WaitForAssertion(() => imagesApiStatus.MarkupMatches(@"<p  id=""ImagesApiHealthStatus"" >
  <br>
  <medium class=""alert alert-danger"">
    Images Api Health Status: Does.Not.Matter</medium>
</p>"), TimeSpan.FromSeconds(2));
    }
}
