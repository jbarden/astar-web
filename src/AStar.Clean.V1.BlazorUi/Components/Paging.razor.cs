using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace AStar.Clean.V1.BlazorUI.Components;

public partial class Paging
{
    [Inject] public NavigationManager? NavigationManager { get; set; }

    [Parameter]
    public int? CurrentPage { get; set; } = 1;

    [Parameter]
    public int? TotalPages { get; set; } = 1;

    [Parameter] public CancellationTokenSource? CancellationTokenSource { get; set; }

    public int Target { get; set; }

    private List<SelectedItem> Items { get; } = new();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        for (var i = 1; i <= TotalPages; i++)
        {
            Items.Add(new() { Text = $"Page {i}", Value = i.ToString(), Active = i == CurrentPage });
        }
    }

    private Task Navigate(SelectedItem e)
    {
        CancellationTokenSource?.Cancel();
        NavigationManager?.NavigateTo($"/images/list/{e.Value}");
        return Task.CompletedTask;
    }
}
