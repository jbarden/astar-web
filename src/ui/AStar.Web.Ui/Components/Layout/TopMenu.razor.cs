using AStar.Web.UI.Shared;
using Blazorise.Localization;
using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Components.Layout;

public partial class TopMenu
{
    private bool topbarVisible = false;

    [Parameter] public EventCallback<bool> ThemeEnabledChanged { get; set; }

    [Parameter] public EventCallback<bool> ThemeGradientChanged { get; set; }

    [Parameter] public EventCallback<bool> ThemeRoundedChanged { get; set; }

    [Parameter] public EventCallback<string> ThemeColorChanged { get; set; }

    [Parameter] public string? LayoutType { get; set; }

    [Parameter] public EventCallback<string> LayoutTypeChanged { get; set; }

    [Inject] protected ITextLocalizerService? LocalizationService { get; set; }

    [CascadingParameter] protected Theme? Theme { get; set; }

    private Task OnThemeColorChanged()
    {
        if(Theme is not null)
        {
            if(Theme.ColorOptions.Primary == SupportedColours.Pink)
            {
                Theme.BackgroundOptions.Primary = SupportedColours.Orange;
                Theme.ColorOptions.Primary = SupportedColours.Orange;
            }
            else
            {
                Theme.BackgroundOptions.Primary = SupportedColours.Pink;
                Theme.ColorOptions.Primary = SupportedColours.Pink;
            }
        }

        Theme?.ThemeHasChanged();

        return Task.CompletedTask;
    }

    private Task OnThemeColorReset()
    {
        if(Theme is not null)
        {
            if(Theme.BackgroundOptions.Primary == SupportedColours.Black)
            {
                Theme.BackgroundOptions.Primary = SupportedColours.White;
                Theme.ColorOptions.Primary = SupportedColours.Black;
            }
            else
            {
                Theme.BackgroundOptions.Primary = SupportedColours.Black;
                Theme.ColorOptions.Primary = SupportedColours.White;
            }
        }

        Theme?.ThemeHasChanged();

        return Task.CompletedTask;
    }
}
