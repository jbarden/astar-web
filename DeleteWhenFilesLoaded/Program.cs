using System.Text.Json;
using AStar.Infrastructure.Data.Data;
using AStar.Infrastructure.Data.Models;

Console.WriteLine(DateTime.UtcNow);

// NOT tested just changed to compile...
var context = new FilesContext();

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
foreach(var fileInfoJb in fileInfoJbs)
{
    ///*C*/onsole.WriteLine(fileInfoJb.FullName);
}

void UpdateFilesInDatabase(IReadOnlyCollection<FileDetail> fileList, FilesContext context)
{
    foreach(var fileInfo in fileList.Where(fileInfo => context.Files.FirstOrDefault(x =>
                 x.FileName == fileInfo.FileName && x.DirectoryName == fileInfo.DirectoryName) is null))
    {
        _ = context.Files.Add(fileInfo);
    }

    foreach(var fileInfo in fileList.Where(fileInfo => context.Files.FirstOrDefault(x =>
                 x.FileName == fileInfo.FileName && x.DirectoryName == fileInfo.DirectoryName) is not null))
    {
        var existing = context.Files.FirstOrDefault(x =>
                x.FileName == fileInfo.FileName && x.DirectoryName == fileInfo.DirectoryName);
        if(existing != null)
        {
            _ = context.Files.Remove(existing);
            _ = context.Files.Add(fileInfo);
        }
    }

    _=context.SaveChanges();
}
