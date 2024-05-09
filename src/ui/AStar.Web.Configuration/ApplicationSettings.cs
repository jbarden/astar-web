namespace AStar.Web.UI.Configuration;

public partial class ApplicationSettings
{
    public bool DetailedErrors { get; set; }

    public Logging Logging { get; set; } = new();

    public string AllowedHosts { get; set; } = "*.*";

    public ApiConfiguration ApiConfiguration { get; set; } = new();

    public ApplicationConfiguration ApplicationConfiguration { get; set; } = new();
}
