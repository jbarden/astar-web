using System.Globalization;
using AStar.Infrastructure.Data;
using AStar.Update.Database.WorkerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace AStar.Update.Database.WorkerService;

public class UpdateDatabaseForAllFiles(ILogger<UpdateDatabaseForAllFiles> logger, IOptions<ApiConfiguration> directories) : BackgroundService
{
    private FilesContext Context
        => new(new DbContextOptionsBuilder<FilesContext>().UseSqlite(ServiceConstants.SqliteConnectionString).Options);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("UpdateDatabaseForAllFiles started at: {RunTime}", DateTimeOffset.Now);
            var startTime = DateTime.UtcNow;
            var endTime = "5:00 AM";

            var duration = DateTime.Parse(endTime, CultureInfo.CurrentCulture).Subtract(startTime);
            if(duration < TimeSpan.Zero)
            {
                duration = duration.Add(TimeSpan.FromHours(24));
            }

            logger.LogInformation("Waiting for: {RunTime} hours before updating the full database.", duration);
            await Task.Delay(duration, stoppingToken);

            while(!stoppingToken.IsCancellationRequested)
            {
                var files = new List<string>();

                GetFiles(directories, files);

                UpdateDirectoryFiles(files, stoppingToken);
                logger.LogInformation("Waiting for: 24 hours before updating the full database again.");
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error occurred in AStar.Update.Database.WorkerService: {ErrorMessage}", ex.Message);
        }
    }

    private void SaveChangesSafely(ILogger<UpdateDatabaseForAllFiles> logger)
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

    private void GetFiles(IOptions<ApiConfiguration> directories, List<string> files)
        => directories.Value.Directories
                            .ToList()
                            .ForEach(dir => GetFilesFromDirectory(dir, files));

    private void RemoveFilesFromDbThatDoNotExistAnyMore(IEnumerable<string> files)
    {
        logger.LogInformation("Starting removal of files deleted from disc outside of the UI.");
        foreach(var file in Context.FileAccessDetails.Where(file => !file.SoftDeleted && !file.SoftDeletePending))
        {
            var fileDetail = Context.Files.Single(f=>f.Id == file.Id);
            if(!files.Contains(Path.Combine(fileDetail.DirectoryName, fileDetail.FileName)))
            {
                _ = Context.Files.Remove(fileDetail);
            }
        }
    }

    private void GetFilesFromDirectory(string dir, List<string> files)
    {
        logger.LogInformation("Getting files in {Directory}", dir);
        files.AddRange(System.IO.Directory.EnumerateFiles(dir, "*.*",
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

    private void ProcessMovedFiles(IEnumerable<string> files, string[] directories, bool run = false)
    {
        if(!run)
        {
            return;
        }

        logger.LogInformation("Starting update of files that have moved");
        foreach(var directory in directories)
        {
            foreach(var file in files.Where(file => file.StartsWith(directory)))
            {
                var lastIndexOf = file.LastIndexOf('\\');
                var directoryName = file[..lastIndexOf];
                var fileName = file[++lastIndexOf..];

                Infrastructure.Models.FileDetail? movedFile = Context.Files.FirstOrDefault(f => f.DirectoryName.StartsWith(directory) && f.DirectoryName != directoryName && f.FileName == fileName);
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

    private void UpdateExistingFile(string directoryName, string fileName, Infrastructure.Models.FileDetail fileFromDatabase)
    {
        foreach(var file in Context.Files.Where(file => file.FileName == fileName))
        {
            _ = Context.Files.Remove(file);
        }

        SaveChangesSafely(logger);

        var updatedFile = new Infrastructure.Models.FileDetail
        {
            DirectoryName = directoryName,
            Height = fileFromDatabase.Height,
            Width = fileFromDatabase.Width,
            FileName = fileName,
            FileSize = fileFromDatabase.FileSize
        };
        _ = Context.Files.Add(updatedFile);
        var fileAccessDetail = new Infrastructure.Models.FileAccessDetail
        {
            SoftDeleted = false,
            SoftDeletePending = false,
            DetailsLastUpdated = DateTime.UtcNow,
            Id = updatedFile.Id
        };

        _ = Context.FileAccessDetails.Add(fileAccessDetail);
        logger.LogInformation("File: {FileName} ({OriginalLocation}) appears to have moved since being added to the dB - previous location: {DirectoryName}", fileName, directoryName, fileFromDatabase.DirectoryName);
    }

    private void AddNewFile(string file)
    {
        try
        {
            var fileInfo = new FileInfo(file);
            var fileDetail = new Infrastructure.Models.FileDetail(fileInfo)
            {
                FileName = fileInfo.Name,
                DirectoryName = fileInfo.DirectoryName!,
                FileSize = fileInfo.Length
            };

            if(fileDetail.IsImage)
            {
                var image = SKImage.FromEncodedData(file);
                if(image is null)
                {
                    File.Delete(file);
                }
                else
                {
                    fileDetail.Height = image.Height;
                    fileDetail.Width = image.Width;
                }
            }

            var fileAccessDetail = new Infrastructure.Models.FileAccessDetail
            {
                SoftDeleted = false,
                SoftDeletePending = false,
                DetailsLastUpdated = DateTime.UtcNow,
                Id = fileDetail.Id
            };

            _ = Context.Files.Add(fileDetail);
            _ = Context.FileAccessDetails.Add(fileAccessDetail);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error retrieving file '{File}' details", file);
        }
    }
}
