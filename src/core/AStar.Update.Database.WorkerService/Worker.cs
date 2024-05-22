using AStar.Infrastructure.Data;
using AStar.Update.Database.WorkerService.Models;
using AStar.Web.Domain;
using AStar.Web.Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace AStar.Update.Database.WorkerService;

public class Worker(ILogger<Worker> logger, IOptions<ApiConfiguration> directories) : BackgroundService
{
    private static readonly FilesContext Context
                        = new(new DbContextOptionsBuilder<FilesContext>().UseSqlite("Data Source=F:\\files-db\\files.db").Options);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Context.Database.EnsureCreated();
        while(!stoppingToken.IsCancellationRequested)
        {
            ProcessFilesMarkedForDeletion();
            logger.LogInformation("here we are");
            logger.LogInformation("AStar.Update.Database running at: {RunTime}", DateTimeOffset.Now);
            var files = new List<string>();

            directories.Value.Directories
                                    .ToList()
                                    .ForEach(dir => GetFilesFromDirectory(dir, files));

            UpdateDirectoryFiles(files, stoppingToken);
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private static void RemoveFilesFromDbThatDoNotExistAnyMore(IEnumerable<string> files, bool run = false)
    {
        if(!run)
        {
            return;
        }

        foreach(var file in Context.Files.Where(file => !file.SoftDeleted && !file.SoftDeletePending))
        {
            if(!files.Contains(Path.Combine(file.DirectoryName, file.FileName)))
            {
                _ = Context.Files.Remove(file);
            }
        }
    }

    private static void SaveChangesSafely(ILogger<Worker> logger)
    {
        try
        {
            _ = Context.SaveChanges();
        }
        catch(Exception ex)
        {
            if(!ex.Message.StartsWith("The database operation was expected to affect"))
            {
                logger.LogError(ex, "Error: {Error} occurred whilst saving changes - probably 'no records affected'", ex.Message);
            }
        }
    }

    private void GetFilesFromDirectory(string dir, List<string> files)
    {
        logger.LogInformation("Getting files in {Directory}", dir);
        files.AddRange(Directory.EnumerateFiles(dir, "*.*",
                            new EnumerationOptions()
                            {
                                RecurseSubdirectories = true,
                                IgnoreInaccessible = true
                            }
                        ));
        logger.LogInformation("Got files in {Directory}", dir);
    }

    private void UpdateDirectoryFiles(IEnumerable<string> files, CancellationToken stoppingToken)
    {
        var filesInDb = Context.Files.Select(file => Path.Combine(file.DirectoryName, file.FileName));

        ProcessNewFiles(files, filesInDb, stoppingToken);
        ProcessMovedFiles(files, directories.Value.Directories);

        RemoveFilesFromDbThatDoNotExistAnyMore(files);

        SaveChangesSafely(logger);
    }

    private void ProcessFilesMarkedForDeletion()
    {
        Context.Files.Where(file => file.SoftDeletePending)
                     .ToList()
                     .ForEach(file =>
                     {
                         if(File.Exists(Path.Combine(file.DirectoryName, file.FileName)))
                         {
                             File.Delete(Path.Combine(file.DirectoryName, file.FileName));
                         }

                         file.SoftDeleted = true;
                         file.SoftDeletePending = false;
                         file.DetailsLastUpdated = DateTime.UtcNow;
                     });

        Context.Files.Where(file => file.HardDeletePending)
                     .ToList()
                     .ForEach(file =>
                     {
                         if(File.Exists(Path.Combine(file.DirectoryName, file.FileName)))
                         {
                             File.Delete(Path.Combine(file.DirectoryName, file.FileName));
                         }

                         file.SoftDeleted = true;
                         file.SoftDeletePending = false;
                         file.DetailsLastUpdated = DateTime.UtcNow;
                     });

        foreach(var file in Context.Files.Where(file => file.HardDeletePending))
        {
            if(File.Exists(Path.Combine(file.DirectoryName, file.FileName)))
            {
                File.Delete(Path.Combine(file.DirectoryName, file.FileName));
            }

            _ = Context.Files.Remove(file);
        }

        _ = Context.SaveChanges();
    }

    private void ProcessMovedFiles(IEnumerable<string> files, string[] directories, bool run = false)
    {
        if(!run)
        {
            return;
        }

        foreach(var directory in directories)
        {
            foreach(var file in files.Where(file => file.StartsWith(directory)))
            {
                var lastIndexOf = file.LastIndexOf('\\');
                var directoryName = file[..lastIndexOf];
                var fileName = file[++lastIndexOf..];

                var movedFile = Context.Files.FirstOrDefault(f => f.DirectoryName.StartsWith(directory) && f.DirectoryName != directoryName && f.FileName == fileName);
                if(movedFile != null)
                {
                    UpdateExistingFile(directoryName, fileName, movedFile);
                }
            }
        }
    }

    private void ProcessNewFiles(IEnumerable<string> files, IQueryable<string> filesInDb, CancellationToken stoppingToken)
    {
        var counter = 0;
        var notInTheDatabase = files.Except(filesInDb).ToList();
        foreach(var file in notInTheDatabase)
        {
            if(stoppingToken.IsCancellationRequested)
            {
                _ = Context.SaveChanges();
                break;
            }

            AddNewFile(file);
            counter++;

            if(counter == 1_000)
            {
                counter = 0;
                _ = Context.SaveChanges();
                logger.LogInformation("Updating the database.");

                logger.LogInformation("File {FileName} has been added to the database.", file);
            }
        }
    }

    private void UpdateExistingFile(string directoryName, string fileName, FileDetail fileFromDatabase)
    {
        foreach(var file in Context.Files.Where(file => file.FileName == fileName))
        {
            _ = Context.Files.Remove(file);
        }

        SaveChangesSafely(logger);

        var updatedFile = new FileDetail
        {
            DirectoryName = directoryName,
            SoftDeleted = false,
            SoftDeletePending = false,
            Height = fileFromDatabase.Height,
            Width = fileFromDatabase.Width,
            FileName = fileName,
            DetailsLastUpdated = DateTime.UtcNow,
            LastViewed = fileFromDatabase.LastViewed,
            FileSize = fileFromDatabase.FileSize
        };

        _ = Context.Files.Add(updatedFile);
        logger.LogInformation("File: {FileName} ({OriginalLocation}) appears to have moved since being added to the dB - previous location: {DirectoryName}", fileName, directoryName, fileFromDatabase.DirectoryName);
    }

    private void AddNewFile(string file)
    {
        try
        {
            var fileInfo = new FileInfo(file);
            var fileDetail = new FileDetail(fileInfo)
            {
                DetailsLastUpdated = DateTime.UtcNow,
                FileName = fileInfo.Name,
                DirectoryName = fileInfo.DirectoryName!,
                FileSize = fileInfo.Length
            };

            if(fileDetail.IsImage)
            {
                var image = SKImage.FromEncodedData(file);
                fileDetail.Height = image.Height;
                fileDetail.Width = image.Width;
            }

            _ = Context.Files.Add(fileDetail);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error retrieving file '{File}' details", file);
        }
    }
}
