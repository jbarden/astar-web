using AStar.Web.Domain;
using Microsoft.EntityFrameworkCore;
using static AStar.Utilities.StringExtensions;

namespace AStar.Infrastructure.Data;

/// <summary>
///
/// </summary>
public static class FilesContextExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="startingFolder"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> FilterBySearchFolder(this DbSet<FileDetail> files, string startingFolder, bool recursive)
        => startingFolder.IsNullOrWhiteSpace()
                ? []
                : GetFiles(files, startingFolder, recursive);

    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="startingFolder"></param>
    /// <param name="recursive"></param>
    /// <param name="searchType"></param>
    /// <param name="includeSoftDeleted"></param>
    /// <param name="includeMarkedForDeletion"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> GetMatchingFiles(this DbSet<FileDetail> files, string startingFolder, bool recursive, string searchType, bool includeSoftDeleted, bool includeMarkedForDeletion)
        => files
                .FilterBySearchFolder(startingFolder, recursive)
                .FilterSoftDeleted(includeSoftDeleted)
                .FilterMarkedForDeletion(includeMarkedForDeletion)
                .FilterImagesIfApplicable(searchType);

    private static IEnumerable<FileDetail> GetFiles(DbSet<FileDetail> files, string startingFolder, bool recursive)
        => recursive
                ? files.Where(file => file.DirectoryName.StartsWith(startingFolder))
                : (IEnumerable<FileDetail>)files.Where(file => file.DirectoryName.Equals(startingFolder));

    private static IEnumerable<FileDetail> FilterSoftDeleted(this IEnumerable<FileDetail> files, bool includeSoftDeleted) => !includeSoftDeleted ? files.Where(file => !file.SoftDeleted) : files;

    private static IEnumerable<FileDetail> FilterMarkedForDeletion(this IEnumerable<FileDetail> files, bool includeMarkedForDeletion) => !includeMarkedForDeletion ? files.Where(file => !file.DeletePending) : files;
}
