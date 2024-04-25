﻿namespace AStar.ImagesAPI.Models;

public class FileSize
{
    private FileSize(long fileLength, long height, long width, string checksumValue)
    {
        FileLength = fileLength;
        Height = height;
        Width = width;
        ChecksumValue = checksumValue;
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
    /// Gets the file Checksum value property.
    /// </summary>
    public string ChecksumValue { get; }

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
    public static FileSize Create(long fileLength, long height, long width, string checksumValue) => new(fileLength, height, width, checksumValue);
}
