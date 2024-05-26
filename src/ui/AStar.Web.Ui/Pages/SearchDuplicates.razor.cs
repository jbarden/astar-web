using AStar.Web.UI.ApiClients.FilesApi;
using AStar.Web.UI.Services;
using AStar.Web.UI.Shared;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Pages;

public partial class SearchDuplicates
{
    private const string PREVIOUS = "previous";
    private const string NEXT = "next";
    private string startingFolder = @"f:\wallhaven\named";
    private int groupsPerPage = 10;
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
#pragma warning disable S3928 // Parameter names used in ArgumentException constructors should match an existing one
        var sortOrderAsEnum = sortOrder switch
        {
            0 => SortOrder.SizeDescending,
            1 => SortOrder.SizeAscending,
            2 => SortOrder.NameDescending,
            3 => SortOrder.NameAscending,
            _ => throw new ArgumentOutOfRangeException(nameof(sortOrder)),
        };
#pragma warning restore S3928 // Parameter names used in ArgumentException constructors should match an existing one

        var searchParameters = new SearchParameters() { SearchFolder = startingFolder, Recursive = recursiveSearch, SearchType = SearchType.Duplicates, SortOrder = sortOrderAsEnum, CurrentPage = currentPageAsInt, ItemsPerPage = groupsPerPage };
        Logger.LogInformation("Searching for files in: {SearchFolder} - {SortOrder}, and of {SearchType} (Full Search Parameters: {SearchParameters})", startingFolder, sortOrderAsEnum, SearchType.Duplicates, searchParameters);
        FileGroups = await FilesApiClient.GetDuplicateFilesAsync(searchParameters);
        var filesCount = await FilesApiClient.GetDuplicateFilesCountAsync(searchParameters);
        totalPages = (int)Math.Ceiling(filesCount / (decimal)groupsPerPage);
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
                    : navigation.Equals(NEXT) && currentPage.Equals(groupsPerPage.ToString());

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
        if(currentPageAsInt < groupsPerPage)
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

    private async Task MarkForMoving(string fullName)
    {
        var result = await FilesApiClient.MarkForMovingAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.NeedsToMove = true;
            }

            DeletionStatus = result;
        }
    }

    private async Task MarkForHardDeletion(string fullName)
    {
        var result = await FilesApiClient.MarkForHardDeletionAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.HardDeletePending = true;
            }

            DeletionStatus = result;
        }
    }

    private async Task MarkForSoftDeletion(string fullName)
    {
        var result = await FilesApiClient.MarkForSoftDeletionAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.SoftDeletePending = true;
            }

            DeletionStatus = result;
        }
    }

    private async Task UndoMarkForSoftDeletion(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForSoftDeletionAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.SoftDeletePending = false;
            }

            DeletionStatus = result;
        }
    }

    private async Task UndoMarkForHardDeletion(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForHardDeletionAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.HardDeletePending = false;
            }

            DeletionStatus = result;
        }
    }

    private async Task UndoMarkForMoving(string fullName)
    {
        var result = await FilesApiClient.UndoMarkForMovingAsync(fullName);

        foreach(var fileGroup in FileGroups)
        {
            var file = fileGroup.Files.FirstOrDefault(file => file.FullName == fullName);
            if(file != null)
            {
                file.NeedsToMove = false;
            }

            DeletionStatus = result;
        }
    }
}
