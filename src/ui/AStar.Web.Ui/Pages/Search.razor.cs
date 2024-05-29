using AStar.Web.UI.ApiClients.FilesApi;
using AStar.Web.UI.Models;
using AStar.Web.UI.Services;
using AStar.Web.UI.Shared;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class Search
{
    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string startingFolder = @"f:\wallhaven";
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
    private IEnumerable<int> pages = [];

    public IEnumerable<FileInfoDto> Files { get; set; } = [];

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
        if(SearchFilesService is null)
        {
            sortOrderAsEnum = SearchFilesService?.SortOrder switch
            {
                0 => SortOrder.SizeDescending,
                1 => SortOrder.SizeAscending,
                2 => SortOrder.NameDescending,
                3 => SortOrder.NameAscending,
                _ => throw new ArgumentOutOfRangeException(nameof(sortOrder)),
            };
        }
        else
        {
            sortOrderAsEnum = sortOrder switch
            {
                0 => SortOrder.SizeDescending,
                1 => SortOrder.SizeAscending,
                2 => SortOrder.NameDescending,
                3 => SortOrder.NameAscending,
                _ => throw new ArgumentOutOfRangeException(nameof(sortOrder)),
            };
        }
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
        Files = await FilesApiClient.GetFilesAsync(new SearchParameters() { SearchFolder = startingFolder, Recursive = recursiveSearch, SearchType = searchTypeAsEnum, SortOrder = sortOrderAsEnum, CurrentPage = currentPageAsInt, ItemsPerPage = itemsOrGroupsPerPage });
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

    private async Task MarkForMoving(string fullName)
    {
        var result = await FilesApiClient.MarkForMovingAsync(fullName);

        var file = Files.FirstOrDefault(file => file.FullName == fullName);

        if(file != null)
        {
            file.NeedsToMove = true;
        }

        DeletionStatus = result;
    }

    private async Task MarkForHardDeletion(string fullName)
    {
        var result = await FilesApiClient.MarkForHardDeletionAsync(fullName);

        var file = Files.FirstOrDefault(file => file.FullName == fullName);

        if(file != null)
        {
            file.HardDeletePending = true;
        }

        DeletionStatus = result;
    }

    private async Task MarkForSoftDeletion(string fullName)
    {
        var result = await FilesApiClient.MarkForSoftDeletionAsync(fullName);

        var file = Files.FirstOrDefault(file => file.FullName == fullName);

        if(file != null)
        {
            file.SoftDeletePending = true;
        }

        DeletionStatus = result;
    }

    private async Task UndoMarkForSoftDeletion(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForSoftDeletionAsync(fullName);

        var file = Files.FirstOrDefault(file => file.FullName == fullName);

        if(file != null)
        {
            file.SoftDeletePending = false;
        }

        DeletionStatus = result;
    }

    private async Task UndoMarkForHardDeletion(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForHardDeletionAsync(fullName);

        var file = Files.FirstOrDefault(file => file.FullName == fullName);

        if(file != null)
        {
            file.HardDeletePending = false;
        }

        DeletionStatus = result;
    }

    private async Task UndoMarkForMoving(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForMovingAsync(fullName);

        var file = Files.FirstOrDefault(file => file.FullName == fullName);

        if(file != null)
        {
            file.NeedsToMove = false;
        }

        DeletionStatus = result;
    }
}
