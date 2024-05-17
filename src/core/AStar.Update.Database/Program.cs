using AStar.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AStar.Update.Database;

internal static class Program
{
    private static readonly FilesContext Context = new(new DbContextOptionsBuilder<FilesContext>()
            .UseSqlite("Data Source=F:\\files-db\\files.db")
            .Options);

    static Program()
    {
    }

    private static void Main()
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        var files = Directory.EnumerateFiles(@"f:\wallhaven", "*.*", new EnumerationOptions(){RecurseSubdirectories = true, IgnoreInaccessible = true});
#pragma warning restore S1075 // URIs should not be hardcoded
        foreach(var file in files)
        {
            var fileDividerIndex = file.LastIndexOf('\\');
            var directoryName = file[..fileDividerIndex];
            var fileName = file[++fileDividerIndex..];
            if(Context.Files.FirstOrDefault(f => f.DirectoryName == directoryName && f.FileName == fileName) is null)
            {
                if(Context.Files.FirstOrDefault(f => f.FileName == fileName) is not null)
                {
                    Console.WriteLine($"File: {file} appears to have moved since being added to the dB - previous location: {directoryName}");
                }
                else
                {
                    Console.WriteLine("Needs adding");
                    Console.WriteLine(file);
                }
            }
            else
            {
                Console.WriteLine($"File: {file} exists in the dB");
            }
        }
    }
}
