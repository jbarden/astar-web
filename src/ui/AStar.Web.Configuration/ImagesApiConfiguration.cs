﻿using System.Text.Json;

namespace AStar.Web.UI.Configuration;

public partial class ImagesApiConfiguration
{
    public string BaseUrl { get; set; } = "https://not.set.com";

    public override string ToString() => JsonSerializer.Serialize(this);
}
