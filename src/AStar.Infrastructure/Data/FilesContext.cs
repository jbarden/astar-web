using AStar.Clean.V1.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace AStar.Infrastructure.Data;

/// <summary>
/// The FileContext class/
/// </summary>
public class FilesContext : DbContext
{
    /// <summary>
    /// The list of files in the dB.
    /// </summary>
    public DbSet<FileDetail> Files { get; set; } = null!;

    /// <summary>
    /// The list of tags to ignore.
    /// </summary>
    public DbSet<TagToIgnore> TagsToIgnore { get; set; } = null!;

    /// <summary>
    /// The list of tags to ignore completely.
    /// </summary>
    public DbSet<TagToIgnoreCompletely> TagsToIgnoreCompletely { get; set; } = null!;

    /// <summary>
    /// The overriden OnConfiguring method.
    /// </summary>
    /// <param name="optionsBuilder">
    /// </param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Filename=c:\\temp\\Files.db");

    /// <summary>
    /// The overriden OnModelCreating method.
    /// </summary>
    /// <param name="modelBuilder">
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FileDetail>().HasKey(vf => new { vf.FileName, vf.DirectoryName });
        _ = modelBuilder.Entity<TagToIgnore>().HasKey(tag => tag.Value);
        _ = modelBuilder.Entity<TagToIgnoreCompletely>().HasKey(tag => tag.Value);
    }
}
