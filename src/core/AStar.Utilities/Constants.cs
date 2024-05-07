using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AStar.Utilities;

/// <summary>
/// The <see href="Constants"></see>see> class contains static / constant properties to simplify and centralise various settings.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Returns an instance of <see href="JsonSerializerOptions"></see> configured with the Web defaults.
    /// </summary>
    public static JsonSerializerOptions WebDeserialisationSettings => new(JsonSerializerDefaults.Web);
}
