using AStar.Clean.V1.DomainModel.Types;

namespace AStar.Clean.V1.DomainModel.Functions;

public static class DirectoryNameExtensions
{
    public static bool HasValue(this DirectoryName tagToUse) => !tagToUse.HasNoValue();

    public static bool HasNoValue(this DirectoryName tagToUse) => string.IsNullOrWhiteSpace(tagToUse.Value);
}
