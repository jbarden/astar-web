using System.Text.Json;
using AStar.Clean.V1.DomainModel;
using AStar.Clean.V1.Infrastructure.Data;

namespace DeleteWhenFilesLoaded;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(DateTime.UtcNow);

        var contextFactory = new FilesDbContextFactory();
        var context = contextFactory.CreateDbContext(args);

        var emptyFiles = File.ReadAllText(@"C:\Users\jason\OneDrive\Documents\FileLists\empty-files.json");
        var emptyFilesList = JsonSerializer.Deserialize<List<FileDetail>>(emptyFiles)!;
        UpdateFilesInDatabase(emptyFilesList, context);
        var files = File.ReadAllText(@"C:\Users\jason\OneDrive\Documents\FileLists\files.list.json");
        var filesList = JsonSerializer.Deserialize<List<FileDetail>>(files)!;
        UpdateFilesInDatabase(filesList, context);

        const int take = 50;
        const int skip = 300;
        var fileInfoJbs = context.Files
            .Where(f => f.DirectoryName.Contains("Wallhaven\\named\\E"))
            .OrderBy(f => f.DirectoryName)
            .ThenBy(f => f.FileName)
            .Skip(skip)
            .Take(take);
        foreach (var fileInfoJb in fileInfoJbs)
        {
            ///*C*/onsole.WriteLine(fileInfoJb.FullName);
        }
    }

    private static void UpdateFilesInDatabase(IReadOnlyCollection<FileDetail> fileList, FilesDbContext context)
    {
        foreach (var fileInfo in fileList.Where(fileInfo => context.Files.FirstOrDefault(x =>
                     x.FileName == fileInfo.FileName && x.DirectoryName == fileInfo.DirectoryName) is null))
        {
            _ = context.Files.Add(fileInfo);
        }

        foreach (var fileInfo in fileList.Where(fileInfo => context.Files.FirstOrDefault(x =>
                     x.FileName == fileInfo.FileName && x.DirectoryName == fileInfo.DirectoryName) is not null))
        {
            var existing = context.Files.FirstOrDefault(x =>
                x.FileName == fileInfo.FileName && x.DirectoryName == fileInfo.DirectoryName);
            if (existing != null)
            {
                _ = context.Files.Remove(existing);
                _ = context.Files.Add(fileInfo);
            }
        }

        context.SaveChanges();
    }
}
