using System.Reflection;
using AStar.Clean.V1.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace AStar.Clean.V1.Infrastructure.Data;

public class FilesDbContext : DbContext
{
    public FilesDbContext(DbContextOptions<FilesDbContext> options)
        : base(options)
    {
    }

    public DbSet<FileDetail> Files { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ExampleData.SeedData(modelBuilder);
    }
}
