using AStar.Infrastructure.Data;

namespace AStar.Update.Database.WorkerService;

public abstract class WorkerServiceBase : BackgroundService
{
    protected abstract FilesContext Context { get; }

    public abstract Task RunServiceAsync(CancellationToken stoppingToken);

    protected void SaveChangesSafely(ILogger<UpdateDatabaseForAllFiles> logger)
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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await RunServiceAsync(stoppingToken);
}
