using AStar.Clean.V1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AStar.Clean.V1.Files.WorkerService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddHostedService<Worker>());
        _ = builder.ConfigureServices((hostContext, services) =>
        {
            _ = services.AddDbContext<FilesDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=jbfiles;Trusted_Connection=True;"));
            _ = services.AddHostedService<Worker>();
        });
        var host = builder.Build();
        host.Run();
    }
}
