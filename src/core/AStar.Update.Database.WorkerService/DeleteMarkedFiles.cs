using AStar.Infrastructure.Data;
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
        foreach(var file in Context.Files.Where(file => file.HardDeletePending))
        {
            if(File.Exists(Path.Combine(file.DirectoryName, file.FileName)))
            {
                logger.LogInformation("hard-deleting {File}", file.FileName);
                File.Delete(Path.Combine(file.DirectoryName, file.FileName));
            }

            _ = Context.Files.Remove(file);
            _ = Context.SaveChanges();
        }

        logger.LogInformation("Starting removal of files marked for soft deletion");
        Context.Files.Where(file => file.SoftDeletePending)
                     .ToList()
                     .ForEach(file =>
                     {
                         if(File.Exists(Path.Combine(file.DirectoryName, file.FileName)))
                         {
                             logger.LogInformation("Soft-deleting {File}", file.FileName);
                             File.Delete(Path.Combine(file.DirectoryName, file.FileName));
                         }

                         file.SoftDeleted = true;
                         file.SoftDeletePending = false;
                         file.DetailsLastUpdated = DateTime.UtcNow;
                         _ = Context.SaveChanges();
                     });

        _ = Context.SaveChanges();
    }
}
