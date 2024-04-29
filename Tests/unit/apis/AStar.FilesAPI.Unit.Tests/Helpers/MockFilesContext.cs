using AStar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AStar.FilesAPI.Unit.Tests.Helpers;

internal static class MockFilesContext
{
    public static async Task<FilesContext> CreateAsync()
    {
        var options = new DbContextOptionsBuilder<FilesContext>()
            .UseInMemoryDatabase(databaseName: $"Test{Guid.NewGuid()}")
            .Options;

        var mockFilesContext = new FilesContext(options);
        _ = await mockFilesContext.Database.EnsureCreatedAsync();
        AddMockFiles(mockFilesContext);
        _ = await mockFilesContext.SaveChangesAsync();

        return mockFilesContext;
    }

    private static void AddMockFiles(FilesContext mockFilesContext)
    {
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory1" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory2" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory3" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory4" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory5" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory6" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory7" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory8" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory9" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory10" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory11" });
        _ = mockFilesContext.Files.Add(new() { FileName = "File 1.jpg", DirectoryName = @"c:\temp\directory12" });
    }
}
