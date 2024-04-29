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
        cut.MarkupMatches(@"<h1 class=""h1 mb-3"" >Welcome!</h1>
<div class=""b-loading-indicator-wrapper b-loading-indicator-wrapper-busy b-loading-indicator-wrapper-relative"" >
  <p  >
    Welcome to AStar Development's Web site. This site is intended as a coding playground and is not a serious site!
  </p>
  <h1 class=""h1 mb-3"" >API Statuses</h1>
  <div class=""alert alert-info"" role=""alert"" >
    <p  >
      Below are the results of some basic checks to the backend APIs. Over time, the checks will be extended.
    </p>
    <h2 class=""h2 mb-2"" >Files API Status</h2>
    <p  >
      <br>
    </p>
    <div class=""alert alert-warning"">Files Api Health Status: Checking...please wait...</div>
    <p></p>
    <h2 class=""h2 mb-2"" >Images API Status</h2>
    <p  >
      <br>
    </p>
    <div class=""alert alert-warning"">Images Api Health Status: Checking...please wait...</div>
    <p></p>
  </div>
  <div class=""b-loading-indicator-overlay b-loading-indicator-overlay-relative d-flex justify-content-center align-items-center"" style=""background-color:rgba(255, 255, 255, 0.7)"">
    <svg viewBox=""0 0 128 128"" height=""64px"">
      <g>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#000000""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(45 64 64)""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(90 64 64)""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(135 64 64)""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(180 64 64)""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(225 64 64)""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(270 64 64)""></path>
        <path d=""M38.52 33.37L21.36 16.2A63.6 63.6 0 0 1 59.5.16v24.3a39.5 39.5 0 0 0-20.98 8.92z"" fill=""#c0c0c0"" transform=""rotate(315 64 64)""></path>
        <animateTransform attributeName=""transform"" type=""rotate"" values=""0 64 64;45 64 64;90 64 64;135 64 64;180 64 64;225 64 64;270 64 64;315 64 64"" calcMode=""discrete"" dur=""720ms"" repeatCount=""indefinite""></animateTransform>
      </g>
    </svg>
  </div>
</div>");
    }

    [Fact]
    public void DisplayTheSuccessfulApiStatusWhenApplicablle()
    {
        var mock = Services.AddMockHttpClient();
        mock.When("/health/live").RespondJson(new HealthStatusResponse() { Status = "Healthy" });

        var cut = RenderComponent<Dashboard>();
        cut.MarkupMatches(@"<h1 class=""h1 mb-3"" >Welcome!</h1>
<div class=""b-loading-indicator-wrapper"" >
  <p  >
    Welcome to AStar Development's Web site. This site is intended as a coding playground and is not a serious site!
  </p>
  <h1 class=""h1 mb-3"" >API Statuses</h1>
  <div class=""alert alert-info"" role=""alert"" >
    <p  >
      Below are the results of some basic checks to the backend APIs. Over time, the checks will be extended.
    </p>
    <h2 class=""h2 mb-2"" >Files API Status</h2>
    <p  >
      <br>
    </p>
    <div id=""FilesApiHealthStatus"" class=""alert alert-success"">Files Api Health Status: Healthy</div>
    <p></p>
    <h2 class=""h2 mb-2"" >Images API Status</h2>
    <p  >
      <br>
    </p>
    <div id=""ImagesApiHealthStatus"" class=""alert alert-success"">Images Api Health Status: Healthy</div>
    <p></p>
    <button id=""0HN37NVC70KRN"" type=""button"" class=""btn btn-success"">Recheck APIs</button>
  </div>
</div>");
    }

    [Fact]
    public void DisplayTheFailedApiStatusWhenApplicablle()
    {
        var mock = Services.AddMockHttpClient();
        mock.When("/health/live").RespondJson(new HealthStatusResponse() { Status = "Does.Not.Matter" });

        var cut = RenderComponent<Dashboard>();
        var divs = cut.Find("button");
        Console.WriteLine(divs);

        cut.MarkupMatches(@"<h1 class=""h1 mb-3"" >Welcome!</h1>
<div class=""b-loading-indicator-wrapper"" >
  <p  >
    Welcome to AStar Development's Web site. This site is intended as a coding playground and is not a serious site!
  </p>
  <h1 class=""h1 mb-3"" >API Statuses</h1>
  <div class=""alert alert-info"" role=""alert"" >
    <p  >
      Below are the results of some basic checks to the backend APIs. Over time, the checks will be extended.
    </p>
    <h2 class=""h2 mb-2"" >Files API Status</h2>
    <p  >
      <br>
    </p>
    <div class=""alert alert-danger"">Files Api Health Status: Does.Not.Matter</div>
    <p></p>
    <h2 class=""h2 mb-2"" >Images API Status</h2>
    <p  >
      <br>
    </p>
    <div class=""alert alert-danger"">Images Api Health Status: Does.Not.Matter</div>
    <p></p>
    <button id=""0HN37NMN8IPGE"" type=""button"" class=""btn btn-success""  >Recheck APIs</button>
  </div>
</div>");
    }
}
