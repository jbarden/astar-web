using System.Text.Json.Serialization;

namespace AStar.FilesApi.Models;

[JsonConverter(typeof(JsonStringEnumConverter<SortOrder>))]
public enum SortOrder
{
    SizeDescending,
    SizeAscending,
    NameDescending,
    NameAscending
}
