﻿namespace AStar.FilesApi.Models;

public class FileSize
{
    private FileSize(long fileLength, long height, long width)
    {
        FileLength = fileLength;
        Height = height;
        Width = width;
    }

    /// <summary>
    /// Gets the file length property.
    /// </summary>
    public long FileLength { get; }

    /// <summary>
    /// Gets the file height property.
    /// </summary>
    public long Height { get; }

    /// <summary>
    /// Gets the file width property.
    /// </summary>
    public long Width { get; }

    /// <summary>
    /// The Create method will return a populated instance of the <see cref="FileSize" /> class.
    /// </summary>
    /// <param name="fileLength">
    /// The length of the file.
    /// </param>
    /// <param name="height">
    /// The height of the file if an image.
    /// </param>
    /// <param name="width">
    /// The width of the file if an image.
    /// </param>
    /// <param name="checksumValue">
    /// The checksum value for the file.
    /// </param>
    /// <returns>
    /// A populated instance of <see cref="FileSize" />.
    /// </returns>
    public static FileSize Create(long fileLength, long height, long width) => new(fileLength, height, width);
}
