using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AStar.Clean.V1.Infrastructure.Data;

/// <summary>
/// This factory is used at design-time.
/// </summary>
[ExcludeFromCodeCoverage]
public class FilesDbContextFactory : IDesignTimeDbContextFactory<FilesDbContext>
{
    public FilesDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FilesDbContext>();
        var databasePath = Path.Combine(@"c:\temp", "Files.db");

        _ = optionsBuilder.UseSqlite($@"Data Source={databasePath}");

        return new(optionsBuilder.Options);
    }
}
