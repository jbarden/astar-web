using AStar.CodeGenerators;

namespace AStar.Web.UI.Configuration;

[GenerateToString]
public partial class ApplicationConfiguration
{
    public int PaginationPageDefaultPreAndPostCount { get; set; }
}
