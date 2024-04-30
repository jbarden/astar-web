using AStar.FilesApi.Config;
using AStar.FilesApi.Models;
using AStar.Web.Domain;

namespace AStar.FilesApi.Files;

public static class EnumerableExtensions
{
    public static IEnumerable<FileDetail> FilterImagesIfApplicable(this IEnumerable<FileDetail> files, SearchType searchType)
        => searchType == SearchType.All ? files : files.Where(file => file.IsImage);

    public static int GroupDuplicates(this IEnumerable<FileDetail> files)
    {
        var duplicatesBySize = files.AsEnumerable()
                .GroupBy(file => FileSize.Create(file.FileSize, file.Height, file.Width),
                    new FileSizeEqualityComparer()).Where(files => files.Count() > 1)
                .ToArray();

        return duplicatesBySize.Length;
    }
}
