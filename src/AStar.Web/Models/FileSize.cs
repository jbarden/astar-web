﻿namespace AStar.Web.Models;

public class FileSize
{
    /// <summary>
    /// Gets the file length property.
    /// </summary>
    public long FileLength { get; set; }

    /// <summary>
    /// Gets the file height property.
    /// </summary>
    public long Height { get; set; }

    /// <summary>
    /// Gets the file width property.
    /// </summary>
    public long Width { get; set; }

    /// <summary>
    /// Gets the file Checksum value property.
    /// </summary>
    public string? ChecksumValue { get; set; }
}
