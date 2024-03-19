using AStar.Clean.V1.Files.API.Config;
using AStar.Clean.V1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AStar.Clean.V1.Files.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder RegisterDbContext(this WebApplicationBuilder builder)
    {
        var databaseTypeFromConfiguration = GetDatabaseTypeFromConfiguration(builder);

        var databaseType = Enum.Parse<Database>(databaseTypeFromConfiguration!);

        switch (databaseType)
        {
            case Database.Sql:
                ConfigureSqlServerDbContext(builder);
                break;

            case Database.SqLite:
                ConfigureSqLite(builder);
                break;

            default:
                throw new ArgumentException($"Database type of {databaseTypeFromConfiguration} is not supported.");
        }

        return builder;
    }

    private static string? GetDatabaseTypeFromConfiguration(WebApplicationBuilder builder)
        => builder.Configuration.GetValue<string>("databaseToUse")
           ?? throw new ArgumentException("Database type could not be retrieved.");

    private static void ConfigureSqlServerDbContext(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        _ = builder.Services.AddDbContext<FilesDbContext>(options => options.UseSqlServer(connectionString));
    }

    private static void ConfigureSqLite(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
        _ = builder.Services.AddDbContext<FilesDbContext>(options => options.UseSqlite(connectionString));
    }
}
