using AStar.Web.Domain;

namespace AStar.Infrastructure;

/// <summary>
///
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="searchType"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> FilterImagesIfApplicable(this IEnumerable<FileDetail> files, string searchType)
        => searchType != "Images" ? files : files.Where(file => file.IsImage);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="files"></param>
    /// <param name="sortOrder"></param>
    /// <returns></returns>
    public static IEnumerable<FileDetail> OrderFiles(this IEnumerable<FileDetail> files, SortOrder sortOrder)
        => sortOrder switch
                     {
                         SortOrder.NameAscending => files.OrderBy(f => f.FileName),
                         SortOrder.NameDescending => files.OrderByDescending(f => f.FileName),
                         SortOrder.SizeAscending => files.OrderBy(f => f.FileSize),
                         SortOrder.SizeDescending => files.OrderByDescending(f => f.FileSize),
                         _ => files,
                     };

    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public static int GroupDuplicates(this IEnumerable<FileDetail> files)
    {
        var duplicatesBySize = files.AsEnumerable()
                .GroupBy(file => FileSize.Create(file.FileSize, file.Height, file.Width),
                    new FileSizeEqualityComparer()).Where(files => files.Count() > 1)
                .ToArray();

        return duplicatesBySize.Length;
    }
}
