using AStar.Web.Domain;
using Microsoft.EntityFrameworkCore;
using static AStar.Utilities.StringExtensions;

namespace AStar.FilesApi.Files;

public static class FilesContextExtensions
{
    public static IEnumerable<FileDetail> FilterBySearchFolder(this DbSet<FileDetail> files, string startingFolder, bool recursive)
        => startingFolder.IsNullOrWhiteSpace()
            ? Enumerable.Empty<FileDetail>()
            : GetFiles(files, startingFolder, recursive);

    private static IEnumerable<FileDetail> GetFiles(DbSet<FileDetail> files, string startingFolder, bool recursive)
        => recursive
                ? files.Where(file => file.DirectoryName.StartsWith(startingFolder))
                : (IEnumerable<FileDetail>)files.Where(file => file.DirectoryName.Equals(startingFolder));
}
