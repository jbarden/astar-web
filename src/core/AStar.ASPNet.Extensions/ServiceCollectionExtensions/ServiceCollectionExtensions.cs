using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using AStar.ASPNet.Extensions.Handlers;
using AStar.Logging.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

namespace AStar.ASPNet.Extensions.ServiceCollectionExtensions;

/// <summary>
/// The <see cref="ServiceCollectionExtensions"/> class contains the method(s) available to configure the pipeline in a consistent manner.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// The <see cref="ConfigureUi"/> will do exactly what it says on the tin...this time around, this is for the UI.
    /// </summary>
    /// <param name="services">An instance of the <see cref="IServiceCollection"/> interface that will be configured with the the Global Exception Handler,
    /// and the controllers (a UI isn't much use without them...).</param>
    /// <returns>The original <see cref="IServiceCollection"/> to facilitate method chaining.</returns>
    /// <seealso href="ConfigureApi"></seealso>
    public static IServiceCollection ConfigureUi(this IServiceCollection services)
    {
        _ = services
                .AddExceptionHandler<GlobalExceptionHandler>()
                .AddControllers();

        return services;
    }

    /// <summary>
    /// The <see cref="ConfigureApi(IServiceCollection)"/> will do exactly what it says on the tin... just, this time, it is API-specific.
    /// </summary>
    /// <param name="services">An instance of the <see cref="IServiceCollection"/> interface that will be configured with the current methods.</param>
    /// <returns>The original <see cref="IServiceCollection"/> to facilitate method chaining.</returns>
    /// <seealso href="ConfigureUi"></seealso>
    /// <seealso href="ConfigureApi(IServiceCollection, OpenApiInfo)"></seealso>
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        _ = services.AddExceptionHandler<GlobalExceptionHandler>();
        //_ = services.AddResponseCaching();
        //_ = services.AddHttpCacheHeaders(
        //                                    (expirationModelOptions) =>
        //                                    {
        //                                        expirationModelOptions.MaxAge = 60;
        //                                        expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Public;
        //                                    },
        //                                    (validationModelOptions) =>
        //                                    {
        //                                        validationModelOptions.MustRevalidate = true;
        //                                        validationModelOptions.VaryByAll = true;
        //                                    }
        //                                );
        _ = services.AddControllers(options =>
                    {
                        options.ReturnHttpNotAcceptable = true;
                        options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                        options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                        for(int i = 0; i < options.OutputFormatters.Count; i++)
                        {
                            Log.Information("Formatter: {Formatter}", options.OutputFormatters[i].GetType());
                            if(options.OutputFormatters[i].GetType() == typeof(StringOutputFormatter))
                            {
                                options.OutputFormatters.RemoveAt(i);
                            }
                            else if(options.OutputFormatters[i].GetType() == typeof(SystemTextJsonOutputFormatter))
                            {
                                _ = ((SystemTextJsonOutputFormatter)options.OutputFormatters[i]).SupportedMediaTypes.Remove("text/json");
                            }
                        }
                    })
                    .ConfigureApiBehaviorOptions(setupAction => setupAction.InvalidModelStateResponseFactory = context =>
                                                {
                                                    var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                                                    var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

                                                    validationProblemDetails.Detail = "See the errors field for details.";
                                                    validationProblemDetails.Instance = context.HttpContext.Request.Path;

                                                    validationProblemDetails.Type = "https://courselibrary.com/modelvalidationproblem";
                                                    validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                                                    validationProblemDetails.Title = "One or more validation errors occurred.";

                                                    return new UnprocessableEntityObjectResult(validationProblemDetails)
                                                    {
                                                        ContentTypes = { "application/problem+json" }
                                                    };
                                                });
        _ = services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.WriteIndented = false;
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Default;
            options.JsonSerializerOptions.AllowTrailingCommas = true;
            options.JsonSerializerOptions.MaxDepth = 10;
            options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        });
        _ = services.AddEndpointsApiExplorer();

        _ = services.AddHealthChecks();

        return services;
    }

    /// <summary>
    /// The <see cref="ConfigureApi(IServiceCollection, OpenApiInfo)"/> will do exactly what it says on the tin... just, this time, it is API-specific.
    /// </summary>
    /// <param name="services">An instance of the <see cref="IServiceCollection"/> interface that will be configured with the current methods.</param>
    /// <param name="openApiInfo">An instance of <see cref="OpenApiInfo"></see> that will be used to configure the SwaggerUI.</param>
    /// <returns>The original <see cref="IServiceCollection"/> to facilitate method chaining.</returns>
    /// <seealso href="ConfigureUi"></seealso>
    /// <seealso href="ConfigureApi(IServiceCollection)"></seealso>
    public static IServiceCollection ConfigureApi(this IServiceCollection services, OpenApiInfo openApiInfo)
        => services
                .ConfigureApi()
                .AddSwaggerGen(c =>
                                {
                                    c.SwaggerDoc(openApiInfo.Version, openApiInfo);
                                    c.EnableAnnotations();
                                });

    /// <summary>
    /// The <see cref="AddLogging"/> will do exactly what it says on the tin...
    /// </summary>
    /// <param name="builder">An instance of the <see cref="WebApplicationBuilder"/> class that will be configured with the current methods.</param>
    /// <param name="externalSettingsFile">
    /// The name (including extension) of the file containing the Serilog Configuration settings.
    /// </param>
    /// <returns>The original <see cref="WebApplicationBuilder"/> to facilitate method chaining.</returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder, string externalSettingsFile = "")
    {
        _ = builder.UseSerilogLogging(externalSettingsFile);

        return builder;
    }
}
