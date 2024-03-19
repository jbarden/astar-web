using AStar.Clean.V1.Files.WorkerService.Services;
using AStar.Clean.V1.Infrastructure.Data;

namespace AStar.Clean.V1.Files.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory factory;
    private readonly IImageService imageService;
    private readonly FilesDbContext filesDbContext;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory factory)
    {
        _logger = logger;
        this.factory = factory;
        imageService = new ImageService();
    }

    protected override void Execute(CancellationToken stoppingToken)
    {
        //filesDbContext = factory.CreateScope().ServiceProvider.GetRequiredService<FilesDbContext>();
        //var fileList = Directory.EnumerateFiles(@"C:\Users\jason_tmhes7y\OneDrive\Pictures\Wallpapers", "*.*", SearchOption.AllDirectories);

        //var counter = 0;
        //foreach (var file in fileList)
        //{
        //    var fileInfo = new FileInfo(file);
        //    Dimensions? dimensions = null;
        //    if (file.IsImage())
        //    {
        //        dimensions = new();
        //        var img = imageService.GetImage(file);
        //        if (img != null)
        //        {
        //            dimensions.Height = img.Height;
        //            dimensions.Width = img.Width;
        //        }
        //    }

        //    var lastIndexOf = file.LastIndexOf(@"\");
        //    var directoryName = file[..lastIndexOf];
        //    var fileName = file[lastIndexOf..];

        //    _ = await filesDbContext.Set<FileInfoJb>().Where(f => f.FileName.Equals(fileName) && f.DirectoryName.Equals(directoryName))
        //        .ExecuteDeleteAsync(stoppingToken);

        //    filesDbContext.FileInfoJbs.Update(new()
        //    {
        //        FileName = fileInfo.Name,
        //        DirectoryName = fileInfo.DirectoryName,
        //        FileLastUpdated = fileInfo.LastWriteTimeUtc,
        //        Size = fileInfo.Length,
        //        Dimensions = dimensions,
        //        DetailsLastUpdated = DateTime.UtcNow
        //    });

        //    if (counter == 15)
        //    {
        //        await filesDbContext.SaveChangesAsync(stoppingToken);
        //        counter = 0;
        //    }
        //    else
        //    {
        //        counter++;
        //    }
        //}
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => throw new NotImplementedException();
}
