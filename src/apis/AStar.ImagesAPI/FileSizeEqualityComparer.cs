﻿using AStar.ImagesAPI.Models;

namespace AStar.ImagesAPI;

internal class FileSizeEqualityComparer : IEqualityComparer<FileSize>
{
    /// <summary>
    /// The Equals method has been overridden to perform the equality check currently required. The equality check is for Height, Width and Length - making this more of an ImageComparer...
    /// </summary>
    /// <param name="leftFileSize">
    /// An instance of the <see cref="FileSize" /> class to compare.
    /// </param>
    /// <param name="rightFileSize">
    /// The other instance of the <see cref="FileSize" /> class to compare.
    /// </param>
    /// <returns>
    /// <c>true</c> if the files are deemed to be the same size, <c>false</c> otherwise.
    /// </returns>
    public bool Equals(FileSize? leftFileSize, FileSize? rightFileSize)
        => (rightFileSize == null && leftFileSize == null)
        || (leftFileSize != null && rightFileSize != null && leftFileSize.Height == rightFileSize.Height &&
            leftFileSize.FileLength == rightFileSize.FileLength
            && leftFileSize.Width == rightFileSize.Width &&
            leftFileSize.ChecksumValue == rightFileSize.ChecksumValue);

    /// <summary>
    /// The GetHashCode has been overridden to return the hash-codes as per the fields compared in the overridden Equals method.
    /// </summary>
    /// <param name="fileSize">
    /// The <see cref="FileSize" /> to calculate the appropriate hash-code for.
    /// </param>
    /// <returns>
    /// The hash-code.
    /// </returns>
    public int GetHashCode(FileSize fileSize)
    {
        var hashCode = fileSize.Height.GetHashCode() ^ fileSize.FileLength.GetHashCode() ^ fileSize.Width.GetHashCode();

        return hashCode.GetHashCode();
    }
}
