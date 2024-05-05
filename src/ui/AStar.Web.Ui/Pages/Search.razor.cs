using AStar.Web.UI.FilesApi;
using AStar.Web.UI.Shared;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class Search
{
    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string startingFolder = @"f:\wallhaven\named\q";
    private int itemsPerPage = 20;
    private int searchType;
    private int sortOrder;
    private int pages;
    private int initialPages;
    private int finalPages;
    private LoadingIndicator loadingIndicator = new();

    private bool accordionItem1Visible = true;

    private bool accordionItem3Visible = false;

    private string currentPage = "1";

    [Inject]
    private FilesApiClient FilesApiClient { get; set; } = default!;

    [Inject]
    private ILogger<Search> Logger { get; set; } = default!;

    private async Task SubmitHorizontalForm()
    {
        await loadingIndicator.Show();
        var filesCount = await FilesApiClient.GetFilesCountAsync(new SearchParameters() { SearchFolder = startingFolder, SearchType = SearchType.Images,SortOrder = SortOrder.SizeDescending });

        pages = (int)Math.Ceiling(filesCount / (decimal)itemsPerPage);
        if(pages > 5)
        {
            initialPages = 5;
        }

        if(pages - 5 > 0)
        {
            finalPages = pages - 5;
        }
        Logger.LogInformation("FilesApiClient fileCount: {FileCount}", filesCount);
        accordionItem1Visible = false;
        accordionItem3Visible = true;
        await loadingIndicator.Hide();
    }

    private bool IsActive(string page) => currentPage == page;

    private bool IsPageNavigationDisabled(string navigation)
    {
        if(navigation.Equals(PREVIOUS))
        {
            return currentPage.Equals("1");
        }
        else if(navigation.Equals(NEXT))
        {
            return currentPage.Equals(itemsPerPage.ToString());
        }
        return false;
    }

    private void Previous()
    {
        var currentPageAsInt = int.Parse(currentPage);
        if(currentPageAsInt > 1)
        {
            currentPage = (currentPageAsInt - 1).ToString();
        }
    }

    private void Next()
    {
        var currentPageAsInt = int.Parse(currentPage);
        if(currentPageAsInt < itemsPerPage)
        {
            currentPage = (currentPageAsInt + 1).ToString();
        }
    }

    private void SetActive(string page)
                    => currentPage = page; // The re-query will happen here
}
