using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

namespace AStar.ASPNet.Extensions.ServiceCollectionExtensions;

public class ServiceCollectionExtensionsShould
{
    public ServiceCollectionExtensionsShould()
    {
        var args = Array.Empty<string>();
        SUT = WebApplication.CreateBuilder(args);
    }

    private WebApplicationBuilder SUT { get; }

    [Fact]
    public void AddLoggingCorrectly()
    {
        SUT.AddLogging("astar-logging-settings.json");

        var provider = SUT.Services.BuildServiceProvider();
        var logger = provider.GetService<ILogger<ServiceCollectionExtensionsShould>>();
        logger.Should().NotBeNull();
    }

    [Fact]
    public void AddTheGlobalExceptionHandlerCorrectly()
    {
        SUT.Services.ConfigureApi();

        var provider = SUT.Services.BuildServiceProvider();
        var globalExceptionHandler = provider.GetService<IExceptionHandler>();
        globalExceptionHandler.Should().NotBeNull();
    }

    [Fact]
    public void AddTheSwaggerGenUICorrectly()
    {
        var initialCount = SUT.Services.Count();

        SUT.Services.ConfigureApi(new OpenApiInfo());

        SUT.Services.Count().Should().BeGreaterThan(initialCount, "the count should have increased if the Swagger Gen UI had been added.");
    }
}
