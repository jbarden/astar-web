using AStar.Clean.V1.DomainModel.Types;

namespace AStar.Clean.V1.DomainModel.Functions;

public static class FileNameAndPathExtensions
{
    public static bool FileAlreadyDownloaded(this IEnumerable<FileNameAndPath> files, FileName fileName) => files.ToList().Contains(fileName.Value);
}
