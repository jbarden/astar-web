using System.IO.Abstractions;
using AStar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AStar.FilesApi.StartupConfiguration;

public static class Services
{
    public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
    {
        var  contextOptions = new DbContextOptionsBuilder<FilesContext>()
            .UseSqlite(configuration.GetConnectionString("FilesDb")!)
            .Options;

        _ = services.AddScoped(_ => new FilesContext(contextOptions));
        _ = services.AddSingleton<IFileSystem, FileSystem>();

        var sp = services.BuildServiceProvider();
        var context = sp.GetRequiredService<FilesContext>();
        _ = context.Database.EnsureCreated();

        return services;
    }
}
