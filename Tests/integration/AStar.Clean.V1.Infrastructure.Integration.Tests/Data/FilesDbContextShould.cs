using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AStar.Clean.V1.Infrastructure.Data;

public class FilesDbContextShould
{
    [Fact]
    public void Test1()
    {
        var options = CreateNewContextOptions();
        var context = new FilesDbContext(options);

        _ = context.Files.Add(new());
        _ = context.SaveChanges();

        _ = context.Files.ToList().Count.Should().Be(1);
    }

    private static DbContextOptions<FilesDbContext> CreateNewContextOptions()
    {
        // Create a fresh service provider, and therefore a fresh
        // InMemory database instance.
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        // Create a new options instance telling the context to use an
        // InMemory database and the new service provider.
        var builder = new DbContextOptionsBuilder<FilesDbContext>();
        _ = builder.UseInMemoryDatabase("filesdbinmemory")
            .UseInternalServiceProvider(serviceProvider);

        return builder.Options;
    }
}
