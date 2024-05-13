﻿using System.Text.Json;

namespace AStar.Web.UI.Models;

public class FileInfoDto
{
    public string Name { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public long Height { get; set; }

    public long Width { get; set; }

    public long Size { get; set; }

    /// <summary>
    /// Need to account for decimal points here...
    /// </summary>
    public string SizeForDisplay
                        => Size / 1024 / 1024 > 0
                                ? (Size / 1024D / 1024D).ToString("N2") + " Mb"
                                : (Size / 1024D).ToString("N2") + " Kb";

    public string ChecksumHash { get; set; } = string.Empty;

    public DateTime? Created { get; set; }

    /// <summary>
    /// Gets or sets whether the file has been 'soft deleted'. I know, shocking...
    /// </summary>
    public bool SoftDeleted { get; set; }

    /// <summary>
    /// Gets or sets whether the file has been marked as  'delete pending'. I know, shocking...
    /// </summary>
    public bool DeletePending { get; set; }

    /// <summary>
    /// Returns this object in JSON format.
    /// </summary>
    /// <returns>This object serialized as a JSON object.</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}
