using AStar.FilesApi.Client.SDK.FilesApi;
using AStar.FilesApi.Client.SDK.Models;
using AStar.Web.UI.Services;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class Search
{
    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string startingFolder = @"c:\wallhaven";
    private int itemsOrGroupsPerPage = 10;
    private int searchType;
    private int sortOrder;
    private int totalPages;
    private IReadOnlyCollection<int> pagesForPagination = [];
    private LoadingIndicator loadingIndicator = new();
    private bool accordionItem1Visible = true;
    private bool accordionItem3Visible = false;
    private string currentPage = "1";
    private int currentPageAsInt = 1;
    private bool recursiveSearch = true;
    private bool excludeViewed = true;
    private IEnumerable<int> pages = [];

    public IEnumerable<AStar.FilesApi.Client.SDK.Models.FileDetail> Files { get; set; } = [];

    public IEnumerable<FileAccessDetail> FileAccessDetails { get; set; } = [];

    public string DeletionStatus { get; private set; } = string.Empty;

    [Inject]
    private FilesApiClient FilesApiClient { get; set; } = default!;

    [Inject]
    private PaginationService PaginationService { get; set; } = default!;

    [Inject]
    private ILogger<Search> Logger { get; set; } = default!;

    [Inject]
    private SearchFilesService SearchFilesService { get; set; } = default!;

    private async Task StartSearch() => await SearchForMatchingFiles();

    private async Task SearchForMatchingFiles()
    {
        await loadingIndicator.Show();
        Files = [];
#pragma warning disable S3928 // Parameter names used in ArgumentException constructors should match an existing one
        SortOrder sortOrderAsEnum;

#pragma warning disable IDE0045 // Convert to conditional expression
        sortOrderAsEnum = sortOrder switch
        {
            0 => SortOrder.SizeDescending,
            1 => SortOrder.SizeAscending,
            2 => SortOrder.NameDescending,
            3 => SortOrder.NameAscending,
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder)),
        };
#pragma warning restore IDE0045 // Convert to conditional expression

        var searchTypeAsEnum = searchType switch
        {
            0 => SearchType.Images,
            1 => SearchType.All,
            2 => SearchType.Duplicates,
            _ => throw new ArgumentOutOfRangeException(nameof(searchType)),
        };
#pragma warning restore S3928 // Parameter names used in ArgumentException constructors should match an existing one

        Logger.LogInformation("Searching for files in: {SortOrder}, and of {SearchType}", sortOrderAsEnum, searchTypeAsEnum);
        Files = await FilesApiClient.GetFilesAsync(new SearchParameters() { SearchFolder = startingFolder, Recursive = recursiveSearch, SearchType = searchTypeAsEnum, SortOrder = sortOrderAsEnum, CurrentPage = currentPageAsInt, ItemsPerPage = itemsOrGroupsPerPage, ExcludedViewSettings = new() { ExcludeViewed = excludeViewed } });
        var filesCount = await FilesApiClient.GetFilesCountAsync(new SearchParameters() { SearchFolder = startingFolder, Recursive = recursiveSearch, SearchType = searchTypeAsEnum, SortOrder = sortOrderAsEnum });
        totalPages = (int)Math.Ceiling(filesCount / (decimal)itemsOrGroupsPerPage);
        pagesForPagination = PaginationService.GetPaginationInformation(totalPages, currentPageAsInt);

        pages = Enumerable.Range(1, totalPages).ToList();
        Logger.LogInformation("FilesApiClient fileCount: {FileCount}", filesCount);
        accordionItem1Visible = false;
        accordionItem3Visible = true;
        await loadingIndicator.Hide();
    }

    private async Task OnSelectedValueChanged(int value)
    {
        currentPageAsInt = value;
        currentPage = value.ToString();

        await SetActive(currentPage);
    }

    private bool IsActive(string page) => currentPage == page;

    private bool IsPageNavigationDisabled(string navigation)
        => navigation.Equals(PREVIOUS)
                    ? currentPage.Equals("1")
                    : navigation.Equals(NEXT) && currentPage.Equals(itemsOrGroupsPerPage.ToString());

    private async Task Previous()
    {
        currentPageAsInt = int.Parse(currentPage);
        if(currentPageAsInt > 1)
        {
            currentPage = (currentPageAsInt - 1).ToString();
        }

        await SetActive(currentPage);
    }

    private async Task Next()
    {
        currentPageAsInt = int.Parse(currentPage);
        if(currentPageAsInt < itemsOrGroupsPerPage)
        {
            currentPage = (currentPageAsInt + 1).ToString();
        }

        await SetActive(currentPage);
    }

    private async Task SetActive(string page)
    {
        Files = [];
        currentPage = page;
        currentPageAsInt = int.Parse(currentPage);

        await SearchForMatchingFiles();
    }
}
