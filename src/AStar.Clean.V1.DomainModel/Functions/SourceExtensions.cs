using AStar.Clean.V1.DomainModel.Types;

namespace AStar.Clean.V1.DomainModel.Functions;

public static class SourceExtensions
{
    public static string Filename(this Source source) =>
        string.IsNullOrWhiteSpace(source.Value)
            ? string.Empty
            : source.Value[source.LastSlashIndex()..].Replace(' ', '-').Replace("#", string.Empty);

    private static int LastSlashIndex(this Source header) =>
        string.IsNullOrWhiteSpace(header.Value)
            ? 0
            : header.Value.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
}
