using AStar.Web.UI.FilesApi;
using AStar.Web.UI.ImagesApi;
using AStar.Web.UI.MockMessageHandlers;
using AStar.Web.UI.Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Tests.bUnit;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AStar.Web.UI.Pages;

public class SearchShould : TestContext
{
    public SearchShould()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        Services.AddMockHttpClient();
        Services.AddScoped<FilesApiClient>();
        Services.AddScoped<ImagesApiClient>();
        Services.AddScoped<PaginationService>();
        Services
            .AddBlazoriseTests()
            .AddBootstrap5Providers()
            .AddEmptyIconProvider();
        JSInterop.AddBlazoriseButton();
        Services.AddBlazorise().Replace(ServiceDescriptor.Transient<IComponentActivator, ComponentActivator>());
    }

    [Fact]
    public void LoadTheInitialHTMLWithTheStartingFolderEntryAvailable()
    {
        var cut = RenderComponent<Search>();

        var filesApiStatus = cut.Find("#startingFolder");

        filesApiStatus.MarkupMatches(@"<input type=""text"" class=""form-control"" placeholder=""Please specify the starting folder"" aria-readonly=""false""        id=""startingFolder"" value=""f:\wallhaven\named\q""  >");
    }

    [Fact]
    public void LoadTheInitialHTMLWithTheItemsPerPageEntryAvailable()
    {
        var cut = RenderComponent<Search>();

        var filesApiStatus = cut.Find("#itemsPerPage");

        filesApiStatus.MarkupMatches(@"<input inputmode=""numeric"" class=""form-control"" step=""10""        id=""itemsPerPage"" >");
    }

    [Fact]
    public void LoadTheInitialHTMLWithTheSortOrderEntryAvailable()
    {
        var cut = RenderComponent<Search>();

        var filesApiStatus = cut.Find("#sortOrder");

        filesApiStatus.MarkupMatches(@"<select class=""form-select"" value=""0""         id=""sortOrder"" >
  <option value=""0"" selected="""" >Size Descending</option>
  <option value=""1"" >Size Ascending</option>
  <option value=""2"" >Name Descending</option>
  <option value=""3"" >Name Ascending</option>
</select>");
    }

    [Fact]
    public void LoadTheInitialHTMLWithTheSStartSearchButtonAvailable()
    {
        var cut = RenderComponent<Search>();

        var filesApiStatus = cut.Find("#startSearch");

        filesApiStatus.MarkupMatches(@"<button type=""button"" class=""btn btn-success""  id=""startSearch"" >Submit</button>");
    }
}
