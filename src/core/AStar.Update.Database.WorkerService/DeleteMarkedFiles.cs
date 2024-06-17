using AStar.Infrastructure.Data;
using AStar.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace AStar.Update.Database.WorkerService;

public class DeleteMarkedFiles(ILogger<UpdateDatabaseForAllFiles> logger) : WorkerServiceBase
{
    protected override FilesContext Context
        => new(new DbContextOptionsBuilder<FilesContext>().UseSqlite(ServiceConstants.SqliteConnectionString).Options);

    public override async Task RunServiceAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("DeleteMarkedFiles started at: {RunTime}", DateTimeOffset.Now);
        while(!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ProcessFilesMarkedForDeletion();
                logger.LogInformation("Waiting for an hour before restarting at: {RunTime}", DateTimeOffset.Now.AddHours(1));
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error occurred in AStar.Update.Database.WorkerService: {ErrorMessage}", ex.Message);
            }
        }
    }

    private void ProcessFilesMarkedForDeletion()
    {
        logger.LogInformation("Starting removal of files marked for hard deletion");
        foreach(var fileAccessDetail in Context.FileAccessDetails.Where(file => file.HardDeletePending))
        {
            var actualFile = Context.Files.First(file => file.Id == fileAccessDetail.Id);
            if(File.Exists(Path.Combine(actualFile.DirectoryName, actualFile.FileName)))
            {
                logger.LogInformation("hard-deleting {File}", actualFile.FileName);
                File.Delete(Path.Combine(actualFile.DirectoryName, actualFile.FileName));
            }

            _ = Context.Files.Remove(actualFile);
            _ = Context.SaveChanges();
        }

        logger.LogInformation("Starting removal of files marked for soft deletion");
        Context.FileAccessDetails.Where(fileAccessDetail => fileAccessDetail.SoftDeletePending)
                     .ToList()
                     .ForEach(fileAccessDetail =>
                     {
                         var actualFile = Context.Files.First(file => file.Id == fileAccessDetail.Id);
                         if(File.Exists(Path.Combine(actualFile.DirectoryName, actualFile.FileName)))
                         {
                             logger.LogInformation("hard-deleting {File}", actualFile.FileName);
                             File.Delete(Path.Combine(actualFile.DirectoryName, actualFile.FileName));
                         }

                         fileAccessDetail.SoftDeleted = true;
                         fileAccessDetail.SoftDeletePending = false;
                         fileAccessDetail.DetailsLastUpdated = DateTime.UtcNow;
                         _ = Context.SaveChanges();
                     });

        _ = Context.SaveChanges();
    }
}
