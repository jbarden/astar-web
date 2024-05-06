using AStar.CodeGenerators;

namespace AStar.Web.UI.Configuration;

[GenerateToString]
public partial class Loglevel
{
    public string Default { get; set; } = "Information";

    public string MicrosoftAspNetCore { get; set; } = "Warning";
}
