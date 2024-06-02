﻿using AStar.Web.Domain;

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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public static IEnumerable<FileDetail> FilterImagesIfApplicable(this IEnumerable<FileDetail> files, string searchType, CancellationToken cancellationToken)
                                    => cancellationToken.IsCancellationRequested
                                        ? files
                                        : FilterImagesIfApplicable(files, searchType);

    /// <summary>
    ///
    /// </summary>
    /// <param name="files"></param>
    /// <param name="excludeViewed"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public static IEnumerable<FileDetail> FilterRecentlyViewed(this IEnumerable<FileDetail> files, bool excludeViewed, CancellationToken cancellationToken)
                                    => cancellationToken.IsCancellationRequested
                                        ? files
                                        : FilterRecentlyViewed(files, excludeViewed);

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
    /// Gets the count of duplicates, grouped by Size, Height and Width.
    /// </summary>
    /// <param name="files">The files to return grouped together.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static int GetDuplicatesCount(this IEnumerable<FileDetail> files, CancellationToken cancellationToken)
    {
        if(cancellationToken.IsCancellationRequested)
        {
            return 0;
        }

        var duplicatesBySize = files.AsEnumerable()
                .GroupBy(file => FileSize.Create(file.FileSize, file.Height, file.Width),
                    new FileSizeEqualityComparer()).Where(files => files.Count() > 1)
                .ToArray();

        return duplicatesBySize.Length;
    }

    /// <summary>
    /// Gets the count of duplicates, grouped by Size, Height and Width.
    /// </summary>
    /// <param name="files">The files to return grouped together.</param>
    /// <returns></returns>

    public static IEnumerable<IGrouping<FileSize, FileDetail>> GetDuplicates(this IEnumerable<FileDetail> files)
                                => files
                                    .GroupBy(file => FileSize.Create(file.FileSize, file.Height, file.Width),
                                             new FileSizeEqualityComparer()).Where(files => files.Count() > 1);

    private static IEnumerable<FileDetail> FilterImagesIfApplicable(IEnumerable<FileDetail> files, string searchType)
                                                => searchType != "Images"
                                                        ? files
                                                        : files.Where(file => file.IsImage);

    private static IEnumerable<FileDetail> FilterRecentlyViewed(IEnumerable<FileDetail> files, bool excludeViewed)
                                                => !excludeViewed
                                                        ? files
                                                        : files.Where(file => file.LastViewed is null || file.LastViewed <= DateTime.UtcNow.AddDays(-7));
}
