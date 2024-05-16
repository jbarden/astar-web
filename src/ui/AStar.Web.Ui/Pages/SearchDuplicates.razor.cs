using AStar.Web.UI.FilesApi;
using AStar.Web.UI.Services;
using AStar.Web.UI.Shared;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class SearchDuplicates
{
    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string startingFolder = @"f:\wallhaven\named\q";
    private int itemsPerPage = 30;
    private int sortOrder;
    private int totalPages;
    private IReadOnlyCollection<int> pagesForPagination = [];
    private LoadingIndicator loadingIndicator = new();
    private bool accordionItem1Visible = true;
    private bool accordionItem3Visible = false;
    private string currentPage = "1";
    private int currentPageAsInt = 1;
    private bool recursiveSearch = true;

    public IEnumerable<DuplicateGroup> FileGroups { get; set; } = [];

    public string DeletionStatus { get; private set; } = string.Empty;

    [Inject]
    private FilesApiClient FilesApiClient { get; set; } = default!;

    [Inject]
    private PaginationService PaginationService { get; set; } = default!;

    [Inject]
    private ILogger<Search> Logger { get; set; } = default!;

    private async Task StartSearch() => await SearchForMatchingFiles();

    private async Task SearchForMatchingFiles()
    {
        await loadingIndicator.Show();
        FileGroups = [];
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one
        var sortOrderAsEnum = sortOrder switch
        {
            0 => SortOrder.SizeDescending,
            1 => SortOrder.SizeAscending,
            2 => SortOrder.NameDescending,
            3 => SortOrder.NameAscending,
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder)),
        };
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one

        Logger.LogInformation("Searching for files in: {SortOrder}, and of {SearchType}", sortOrderAsEnum, SearchType.Duplicates);
        FileGroups = await FilesApiClient.GetDuplicateFilesAsync(new SearchParameters() { SearchFolder = startingFolder, SearchType = SearchType.Duplicates, SortOrder = sortOrderAsEnum, CurrentPage = currentPageAsInt, ItemsPerPage = itemsPerPage });
        var filesCount = await FilesApiClient.GetDuplicateFilesCountAsync(new SearchParameters() { SearchFolder = startingFolder, SearchType = SearchType.Duplicates, SortOrder = sortOrderAsEnum });
        totalPages = (int)Math.Ceiling(filesCount / (decimal)itemsPerPage);
        pagesForPagination = PaginationService.GetPaginationInformation(totalPages, currentPageAsInt);

        Logger.LogInformation("FilesApiClient fileCount: {FileCount}", filesCount);
        accordionItem1Visible = false;
        accordionItem3Visible = true;
        await loadingIndicator.Hide();
    }

    private bool IsActive(string page) => currentPage == page;

    private bool IsPageNavigationDisabled(string navigation)
        => navigation.Equals(PREVIOUS)
                    ? currentPage.Equals("1")
                    : navigation.Equals(NEXT) && currentPage.Equals(itemsPerPage.ToString());

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
        if(currentPageAsInt < itemsPerPage)
        {
            currentPage = (currentPageAsInt + 1).ToString();
        }

        await SetActive(currentPage);
    }

    private async Task SetActive(string page)
    {
        FileGroups = [];
        currentPage = page;
        currentPageAsInt = int.Parse(currentPage);

        await SearchForMatchingFiles();
    }

    private async Task MarkForDeletion(string fullName)
    {
        var result = await FilesApiClient.MarkForDeletionAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.DeletePending = true;
            }

            DeletionStatus = result;
        }
    }

    private async Task UndoMarkForDeletion(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForDeletionAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.DeletePending = false;
            }

            DeletionStatus = result;
        }
    }
}
