using AStar.Infrastructure.Data;
using AStar.Web.Domain;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SkiaSharp;

namespace AStar.Update.Database.WorkerService;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private static readonly FilesContext Context = new(new DbContextOptionsBuilder<FilesContext>()
            .UseSqlite("Data Source=F:\\files-db\\files.db")
            .Options);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            Log.Logger.Information("duck you");
            logger.LogInformation("here we are");
            Log.Logger.Information("AStar.Update.Database running at: {RunTime}", DateTimeOffset.Now);

#pragma warning disable S1075 // URIs should not be hardcoded
            var files = Directory.EnumerateFiles(@"f:\wallhaven", "*.*", new EnumerationOptions(){RecurseSubdirectories = true, IgnoreInaccessible = true});
            UpdateDirectoryFiles(files, stoppingToken);
            files = Directory.EnumerateFiles(@"f:\LookAtNow", "*.*", new EnumerationOptions() { RecurseSubdirectories = true, IgnoreInaccessible = true });
            UpdateDirectoryFiles(files, stoppingToken);
#pragma warning restore S1075 // URIs should not be hardcoded
            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }

    private void UpdateDirectoryFiles(IEnumerable<string> files, CancellationToken stoppingToken)
    {
        var counter = 0;
        foreach(var file in files)
        {
            if(stoppingToken.IsCancellationRequested)
            {
                _ = Context.SaveChanges();
                break;
            }

            counter++;
            var fileDividerIndex = file.LastIndexOf('\\');
            var directoryName = file[..fileDividerIndex];
            var fileName = file[++fileDividerIndex..];
            if(Context.Files.FirstOrDefault(f => f.DirectoryName == directoryName && f.FileName == fileName) is null)
            {
                var fileFromDatabase =Context.Files.FirstOrDefault(f => f.FileName == fileName);
                if(fileFromDatabase is null)
                {
                    AddNewFile(file);
                }
                else
                {
                    UpdateExistingFile(directoryName, fileName, fileFromDatabase);
                }
            }
            else
            {
                if(counter == 10_000)
                {
                    Log.Logger.Information("File {FileName} exists in the database.", file);
                }
            }

            if(counter == 10_000)
            {
                counter = 0;
                _ = Context.SaveChanges();
                Log.Logger.Information("Updating the database.");
            }
        }

        _ = Context.SaveChanges();
    }

    private void UpdateExistingFile(string directoryName, string fileName, FileDetail fileFromDatabase)
    {
        _ = Context.Files.Remove(fileFromDatabase);
        var updatedFile = new FileDetail
        {
            DirectoryName = directoryName,
            SoftDeleted = false,
            DeletePending = false,
            Height = fileFromDatabase.Height,
            Width = fileFromDatabase.Width,
            FileName = fileName,
            DetailsLastUpdated = DateTime.Now,
            LastViewed = fileFromDatabase.LastViewed,
            FileSize = fileFromDatabase.FileSize
        };

        _ = Context.Files.Add(updatedFile);
        Log.Logger.Information("File: {FileName} appears to have moved since being added to the dB - previous location: {DirectoryName}", fileName, directoryName);
    }

    private void AddNewFile(string file)
    {
        var fileInfo = new FileInfo(file);
        var fileDetail = new FileDetail(fileInfo);

        if(fileDetail.IsImage)
        {
            var image = SKImage.FromEncodedData(file);
            fileDetail.Height = image.Height;
            fileDetail.Width = image.Width;
        }

        var entity = new FileDetail(fileInfo);
        _ = Context.Files.Add(entity);

        Log.Logger.Information("File {FileName} has been added to the database.", file);
    }
}
