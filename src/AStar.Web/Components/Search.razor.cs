using AStar.Web.Models;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.Components;

public partial class Search
{
    [Parameter]
    public SearchParameters SearchParameters { get; set; } = new();

    [Parameter]
    public EventCallback<bool> OnSubmit { get; set; }

    [Parameter]
    public bool IsBusy { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var searchParameters = SearchParameters;
        if(string.IsNullOrWhiteSpace(searchParameters.SearchFolder))
        {
            searchParameters.SearchFolder = @"F:\WallHaven";
        }

        await base.OnParametersSetAsync();
    }

    private async Task HandleValidSubmit()
    {
        if(OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(true);
        }
    }

    private async Task HandleInvalidSubmit()
    {
        if(OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(false);
        }
    }
}
