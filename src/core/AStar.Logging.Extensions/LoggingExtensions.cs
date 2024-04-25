using AStar.Utilities;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

namespace AStar.Logging.Extensions;

/// <summary>
/// The <see cref="LoggingExtensions" /> class contains, as you might expect, extension methods for configuring Serilog / Application Insights.
/// </summary>
public static class LoggingExtensions
{

/// <summary>
/// 
/// </summary>
/// <param name="services"></param>
/// <returns></returns>
    public static IServiceCollection AddSerilogLogging(this IServiceCollection services)
    {
        _=services.AddApplicationInsightsTelemetry();
        var serviceProvider = services.BuildServiceProvider();
Log.Logger = new LoggerConfiguration()
    .WriteTo.ApplicationInsights(
        serviceProvider.GetRequiredService<TelemetryConfiguration>(),
	TelemetryConverter.Traces)
    .CreateLogger();

    return services;
    }
    /// <summary>
    /// The <see cref="UseSerilogLogging" /> method will add Serilog to the logging providers.
    /// </summary>
    /// <param name="builder">
    /// </param>
    /// <param name="externalSettingsFile">
    /// The name (including extension) of the file containing the Serilog Configuration settings.
    /// </param>
    /// <returns>
    /// The original instance of <see cref="WebApplicationBuilder" /> for further method chaining.
    /// </returns>
    public static WebApplicationBuilder UseSerilogLogging(this WebApplicationBuilder builder, string externalSettingsFile = "")
    {
        if(externalSettingsFile.IsNotNullOrWhiteSpace())
        {
            _ = builder.Configuration.AddJsonFile(path: externalSettingsFile, optional: false, reloadOnChange: true);
        }

        _ = builder.Host
                    .UseSerilog((context, loggerConfig) => loggerConfig
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}")
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext());

        return builder;
    }
}
