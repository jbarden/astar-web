using System.Globalization;
using AStar.Clean.V1.BlazorUI.Models;
using Microsoft.AspNetCore.Components;

namespace AStar.Clean.V1.BlazorUI.Pages.Images;

public partial class List : IDisposable
{
    private const int ItemsPerPage = 4;
    private const int InitialPageWidth = 1450;
    private int width = InitialPageWidth;
    private CancellationTokenSource cancellationTokenSource = new();

    [Parameter]
    public int? CurrentPage { get; set; }

    public SearchParameters SearchParameters { get; set; } = new();

    public IList<FileInfoDto>? ImageList { get; set; }

    public bool Loading { get; set; }

    public bool IsBusy { get; set; }

    private string SidebarWidth => $"width: {width}px;";

    private int TotalPages { get; set; }

    private int ImageCount { get; set; }

    private string? SuccessMessage { get; set; }

    private string? ErrorMessage { get; set; }

    private bool InitialLoadCompleted { get; set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        Logger.LogInformation("Dispose started...");
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadSearchResults();
        OpenNav();
    }

    private void OpenNav() => width = InitialPageWidth;

    private void CloseNav() => width = 50;

    private async Task LoadSearchResults()
    {
        cancellationTokenSource = new();
        ErrorMessage = null;
        ImageList = new List<FileInfoDto>();
        Loading = true;
        if (CurrentPage is null or < 1)
        {
            NavigationManager.NavigateTo("/images/list/1");
            return;
        }

        if (SearchParameters.ItemsPerPage == 0)
        {
            SearchParameters.ItemsPerPage = ItemsPerPage;
            SearchParameters.CurrentPage = 1;
            SearchParameters.RecursiveSubDirectories = true;
        }
        else
        {
            SearchParameters.CurrentPage = CurrentPage ?? 1;
        }

        if (InitialLoadCompleted)
        {
            Logger.LogInformation("Starting search...SearchFolder: {SearchFolder}, CurrentPage: {CurrentPage}, ItemsPerPage: {ItemsPerPage}, SearchType: {SearchType}, RecursiveSubDirectories: {RecursiveSubDirectories}, SortOrder: {SortOrder}", SearchParameters.SearchFolder, SearchParameters.CurrentPage, SearchParameters.ItemsPerPage, SearchParameters.SearchType, SearchParameters.RecursiveSubDirectories, SearchParameters.SortOrder);
            ImageList = await FilesApiClient.GetFilesListAsync(SearchParameters, cancellationTokenSource.Token)
                .ConfigureAwait(false);

            Logger.LogInformation("Got file list. Getting total file count...");

            ImageCount = await FilesApiClient.GetFilesCountAsync(SearchParameters, cancellationTokenSource.Token).ConfigureAwait(false);
            TotalPages = (int)Math.Ceiling((decimal)ImageCount / SearchParameters.ItemsPerPage);
            Logger.LogInformation("Completed search...");
        }

        CloseNav();
        Loading = false;
    }

    private async Task HandleSubmit(bool isValid)
    {
        InitialLoadCompleted = true;
        if (isValid)
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
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;

        try
        {
            await LoadSearchResults();
        }
        catch (Exception ex)
        {
            Logger.LogError(0, ex, ex.Message);
            SuccessMessage = null;
            ErrorMessage = $"Error while searching images: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
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
