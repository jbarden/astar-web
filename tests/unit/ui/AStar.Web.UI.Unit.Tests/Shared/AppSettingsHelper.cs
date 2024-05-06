namespace AStar.Web.UI.Shared;

internal static class AppSettingsHelper
{
    public static string MockApplicationSettings => @"{
  ""DetailedErrors"": true,
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft.AspNetCore"": ""Warning""
    }
  },
  ""AllowedHosts"": ""*"",
  ""apiConfiguration"": {
    ""filesApiConfiguration"": {
      ""baseUrl"": ""https://localhost:7138/""
    },
    ""imagesApiConfiguration"": {
      ""baseUrl"": ""https://localhost:7008/""
    }
  },
  ""applicationConfiguration"": {
    ""paginationPageDefaultPreAndPostCount"": 5
  }
}
";
}
