using System.ComponentModel.DataAnnotations;

namespace AStar.FilesApi.Client.SDK.FilesApi;

/// <summary>
/// The <see href="FilesApiConfiguration"></see> class containing the current configuration settings.
/// </summary>
public class FilesApiConfiguration
{
    /// <summary>
    /// Gets the Section Location for the API configuration from within the appSettings.Json file.
    /// </summary>
    public const string SectionLocation = "ApiConfiguration:FilesApiConfiguration";

    /// <summary>
    /// Gets or sets the Base URL for the API.
    /// </summary>
    [Required]
    public Uri BaseUrl { get; set; } = new("http://not.set.com");
}
