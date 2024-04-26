using AStar.Clean.V1.BlazorUI.Models;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace AStar.Clean.V1.BlazorUI.Components;

public partial class Paging
{
    [Inject] public NavigationManager? NavigationManager { get; set; }

    [Parameter]
    public int? CurrentPage { get; set; } = 1;

    [Parameter]
    public SearchParameters SearchParameters { get; set; } = new();

    [Parameter]
    public int? TotalPages { get; set; } = 1;

    [Parameter] public CancellationTokenSource? CancellationTokenSource { get; set; }

    public string FirstPage { get; set; } = string.Empty;

    public string NextPage { get; set; } = string.Empty;

    public string PreviousPage { get; set; } = string.Empty;

    public string LastPage { get; set; } = string.Empty;

    private List<SelectedItem> Items { get; } = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var uri = new Uri(NavigationManager!.Uri);
        var queryString = uri.Query; // Get the entire query string
        var parsedQuery = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryString);
        if(parsedQuery.TryGetValue("CurrentPage", out var currentPage))
        {
            CurrentPage = int.Parse(currentPage!);
            var searchParameters = SearchParameters;
            var first = SearchParameters;
            var last = SearchParameters;
            var next = SearchParameters;
            if(CurrentPage > 1)
            {
                if(SearchParameters.CurrentPage - 1 < 1)
                {
                    searchParameters.CurrentPage = 1;
                }
                else
                {
                    searchParameters.CurrentPage--;
                }

                PreviousPage = NavigationManager!.Uri.Split('?')[0] + "?" + searchParameters;
            }
            else
            {
                first.CurrentPage = 1;
                PreviousPage = NavigationManager!.Uri.Split('?')[0] + "?" + first;
                FirstPage = PreviousPage;
            }

            if(next.CurrentPage > TotalPages)
            {
                next.CurrentPage = (int)TotalPages;
            }
            else
            {
                next.CurrentPage++;
            }

            NextPage = NavigationManager!.Uri.Split('?')[0] + "?" + next;

            last.CurrentPage = (int)TotalPages!;
            LastPage = NavigationManager!.Uri.Split('?')[0] + "?" + last;
        }

        for(var i = 1; i <= TotalPages; i++)
        {
            Items.Add(new() { Text = $"Page {i}", Value = i.ToString(), Active = i == CurrentPage });
        }
    }

    private Task Navigate(SelectedItem e)
    {
        CancellationTokenSource?.Cancel();
        SearchParameters.CurrentPage = int.Parse(e.Value);
        NavigationManager?.NavigateTo(NavigationManager!.Uri.Split('?')[0] + "?" + SearchParameters);
        return Task.CompletedTask;
    }
}
