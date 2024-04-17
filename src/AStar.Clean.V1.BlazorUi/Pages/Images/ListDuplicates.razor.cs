using System.Globalization;
using AStar.Clean.V1.BlazorUI.Models;
using Microsoft.AspNetCore.Components;

namespace AStar.Clean.V1.BlazorUI.Pages.Images;

public partial class ListDuplicates : IDisposable
{
    private const int InitialPageWidth = 1450;
    private int width = InitialPageWidth;
    private CancellationTokenSource cancellationTokenSource = new();

    public int? CurrentPage { get; set; }

    public SearchParameters SearchParameters { get; set; } = new() { SearchType = SearchType.Duplicates };

    public IList<((long width, long height, long size), IList<FileInfoDto>)> ImageList { get; set; } = [];

    public bool Loading { get; set; }

    public bool IsBusy { get; set; }

    public bool InitialLoad { get; set; }

    public bool SearchCompleted { get; set; } = false;

    private string SidebarWidth => $"width: {width}px;";

    private int TotalPages { get; set; }

    private int ImageCount { get; set; }

    private string? SuccessMessage { get; set; }

    private string? ErrorMessage { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected override async Task OnParametersSetAsync()
    {
        await LoadSearchResultsAsync();
        OpenNav();
    }

    protected override async Task OnInitializedAsync()
    {
        var uri = new Uri(NavigationManager.Uri);
        var queryString = uri.Query; // Get the entire query string
        var parsedQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);
        if(parsedQuery.Count == 0)
        {
            NavigationManager.NavigateTo($"/images/list-duplicates?{SearchParameters}");
        }
        else
        {
            await LoadSearchResultsAsync();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if(!disposing)
        {
            return;
        }

        Logger.LogInformation("Dispose started...");
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    private void OpenNav() => width = InitialPageWidth;

    private void CloseNav() => width = 50;

    private async Task LoadSearchResultsAsync()
    {
        var uri = new Uri(NavigationManager.Uri);
        var queryString = uri.Query; // Get the entire query string
        var parsedQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);
        if(parsedQuery.Count == 0 || ImageList.Count > 0)
        {
            return;
        }

        foreach(var param in parsedQuery)
        {
            Console.WriteLine(param);
            if(param.Key == "SearchFolder")
            {
                SearchParameters.SearchFolder = param.Value!;
            }
            else if(param.Key == "SearchType")
            {
                SearchParameters.SearchType = Enum.Parse<SearchType>(param.Value!);
            }
            else if(param.Key == "RecursiveSubDirectories")
            {
                SearchParameters.RecursiveSubDirectories = bool.Parse(param.Value!);
            }
            else if(param.Key == "CurrentPage")
            {
                SearchParameters.CurrentPage = int.Parse(param.Value!);
                CurrentPage = int.Parse(param.Value!);
            }
            else if(param.Key == "ItemsPerPage")
            {
                SearchParameters.ItemsPerPage = int.Parse(param.Value!);
            }
            else if(param.Key == "MaximumSizeOfThumbnail")
            {
                SearchParameters.MaximumSizeOfThumbnail = int.Parse(param.Value!);
            }
            else if(param.Key == "MaximumSizeOfImage")
            {
                SearchParameters.MaximumSizeOfImage = int.Parse(param.Value!);
            }
            else if(param.Key == "SortOrder")
            {
                SearchParameters.SortOrder = Enum.Parse<SortOrder>(param.Value!);
            }
            else if(param.Key == "CountOnly")
            {
                SearchParameters.CountOnly = bool.Parse(param.Value!);
            }
            else if(param.Key == "IncludeDimensions")
            {
                SearchParameters.IncludeDimensions = bool.Parse(param.Value!);
            }
        }

        cancellationTokenSource = new();
        ErrorMessage = null;
        ImageList = [];
        Loading = true;

        SearchCompleted = false;
        Logger.LogInformation("Starting search...SearchFolder: {SearchFolder}, CurrentPage: {CurrentPage}, ItemsPerPage: {ItemsPerPage}, SearchType: {SearchType}, RecursiveSubDirectories: {RecursiveSubDirectories}, SortOrder: {SortOrder}", SearchParameters.SearchFolder, SearchParameters.CurrentPage, SearchParameters.ItemsPerPage, SearchParameters.SearchType, SearchParameters.RecursiveSubDirectories, SearchParameters.SortOrder);
        ImageList = await FilesApiClient.GetDuplicateFilesListAsync(SearchParameters, cancellationTokenSource.Token).ConfigureAwait(false);
        Logger.LogInformation("Completed search...{ListCount}", ImageList.Count);
        Logger.LogInformation("Got file list. Getting total file count...");
        ImageCount = await FilesApiClient.GetFilesCountAsync(SearchParameters, cancellationTokenSource.Token).ConfigureAwait(false);
        TotalPages = (int)Math.Ceiling((decimal)ImageCount / SearchParameters.ItemsPerPage);
        Logger.LogInformation("Completed search...");
        SearchCompleted = true;

        CloseNav();
        Loading = false;
    }

    private async Task HandleSubmit(bool isValid)
    {
        if(isValid)
        {
            await HandleValidSubmit();
        }
        else
        {
            HandleInvalidSubmit();
        }
    }

    private async Task HandleValidSubmit()
    {
        if(IsBusy)
        {
            return;
        }

        IsBusy = true;
        await LoadSearchResultsAsync();

        IsBusy = false;
    }

    private void HandleInvalidSubmit()
    {
        SuccessMessage = null;
        ErrorMessage = null;
    }

    private string PageTitle()
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        var title = textInfo.ToTitleCase(SearchParameters.SearchFolder.Split('\\').Last().Replace('-', ' '));

        return title;
    }
}
