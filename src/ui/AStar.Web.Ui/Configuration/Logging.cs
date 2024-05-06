using AStar.CodeGenerators;

namespace AStar.Web.UI.Configuration;

[GenerateToString]
public partial class Logging
{
    public Loglevel LogLevel { get; set; } = new();
}
