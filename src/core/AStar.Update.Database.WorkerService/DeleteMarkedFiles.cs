namespace AStar.Update.Database.WorkerService;

public class DeleteMarkedFiles(ILogger<UpdateDatabaseForAllFiles> logger) : WorkerServiceBase
{
    public override async Task RunServiceAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("DeleteMarkedFiles started at: {RunTime}", DateTimeOffset.Now);
        while(!stoppingToken.IsCancellationRequested)
        {
            MoveFilesInNewSubscriptionWallpapersDirectories();
            ProcessFilesMarkedForDeletion();
            logger.LogInformation("Waiting for an hour before restarting at: {RunTime}", DateTimeOffset.Now.AddHours(1));
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private void MoveFilesInNewSubscriptionWallpapersDirectories(bool run = false)
    {
        if(!run)
        {
            return;
        }

        var filesToMove = Context.Files.Where(file => file.DirectoryName.Contains("New-Subscription-Wallpapers"));

        foreach(var fileToMove in filesToMove)
        {
            var newLocation = fileToMove.DirectoryName.Replace("\\New-Subscription-Wallpapers", string.Empty);
            var newNameAndLocation = Path.Combine(newLocation, fileToMove.FileName);
            var newFile = Context.Files.FirstOrDefault(file => file.DirectoryName == newLocation && file.FileName == fileToMove.FileName);

            if(File.Exists(fileToMove.FullNameWithPath))
            {
                File.Move(fileToMove.FullNameWithPath, newNameAndLocation, true);
            }

            if(newFile is null)
            {
                fileToMove.DirectoryName = newLocation;
                _ = Context.Files.Update(fileToMove);
            }
            else
            {
                _ = Context.Files.Remove(fileToMove);
            }

            Console.WriteLine(fileToMove.FullNameWithPath);
        }

        SaveChangesSafely(logger);
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
