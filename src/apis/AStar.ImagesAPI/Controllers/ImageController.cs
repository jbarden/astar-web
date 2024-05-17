﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO.Abstractions;
using AStar.ImagesAPI.Extensions;
using AStar.ImagesAPI.Models;
using AStar.ImagesAPI.Services;
using AStar.Infrastructure.Data;
using AStar.Web.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AStar.ImagesAPI.Controllers;

[Route("api/image")]
[ApiController]
public class ImageController(IFileSystem fileSystem, IImageService imageService, FilesContext context) : ControllerBase
{
    private const int MaximumHeightAndWidthForThumbnail = 850;

    [HttpGet("details", Name = "ImageDetail")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<FileInfoDto> GetImageDetail(string imagePath)
    {
        if(!imagePath.IsImage())
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "Unsupported file type");
        }

        var index = imagePath.LastIndexOf("\\");
        var directory = imagePath[..index];
        var filename = imagePath[(index + 1)..];
        var fileInfoJb = ReadDb(directory, filename);
        if(fileInfoJb is not null)
        {
            fileInfoJb.LastViewed = DateTime.UtcNow;
            try
            {
                _ = context.SaveChanges();
            }
            catch
            {
                // Any error here is not important.
            }
        }

        if(!fileSystem.File.Exists(imagePath))
        {
            return NotFound();
        }

        var file = fileSystem.FileInfo.New(imagePath);
        using var imageFromFile = imageService.GetImage(imagePath);

        return Ok(new FileInfoDto
        {
            FullName = imagePath,
            Name = file.Name,
            Height = imageFromFile.Height,
            Width = imageFromFile.Width,
            Size = file.Length,
            Created = file.CreationTimeUtc
        });
    }

    /// <summary>
    /// Somewhere in here, we rotate images, sometimes... need to dig to see why / where
    /// </summary>
    /// <param name="imagePath">
    /// </param>
    /// <param name="thumbnail">
    /// </param>
    /// <param name="maximumSizeInPixels">
    /// </param>
    /// <param name="resize">
    /// </param>
    /// <returns>
    /// </returns>
    [HttpGet(Name = "Image")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<FileStream> GetImage(string imagePath, bool thumbnail = true, int maximumSizeInPixels = 150,
        bool resize = false)
    {
        if(!imagePath.IsImage())
        {
            return BadRequest("Unsupported file type.");
        }

        if(!fileSystem.File.Exists(imagePath))
        {
            return NotFound();
        }

        var extensionIndex = imagePath.LastIndexOf('.') + 1;
        var extension = imagePath[extensionIndex..];
        using var imageFromFile = imageService.GetImage(imagePath);
        if(thumbnail)
        {
            maximumSizeInPixels = RestrictMaximumSizeInPixels(maximumSizeInPixels);
            var dimensions = ImageDimensions(imageFromFile.Width, imageFromFile.Height, maximumSizeInPixels);

            using var imageThumbnail = ResizeImage(imageFromFile, (int)dimensions.Width!, (int)dimensions.Height!);
            var thumbnailMemoryStream = ToMemoryStream(imageThumbnail);

            return File(thumbnailMemoryStream, $"image/{extension}");
        }

        if(resize)
        {
            maximumSizeInPixels = 1500;
            var dimensions = ImageDimensions(imageFromFile.Width, imageFromFile.Height, maximumSizeInPixels);

            using var imageThumbnail = ResizeImage(imageFromFile, (int)dimensions.Width!, (int)dimensions.Height!);
            var thumbnailMemoryStream = ToMemoryStream(imageThumbnail);

            return File(thumbnailMemoryStream, $"image/{extension}");
        }

        var imageStream = ToMemoryStream(imageFromFile);

        return File(imageStream, $"image/{extension}");
    }

    private static MemoryStream ToMemoryStream(Image b)
    {
        var ms = new MemoryStream();
        b.Save(ms, ImageFormat.Png);
        b.Dispose();
        _ = ms.Seek(0, SeekOrigin.Begin);

        return ms;
    }

    private static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destinationRect = new Rectangle(0, 0, width, height);
        var destinationImage = new Bitmap(width, height);

        destinationImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using var graphics = Graphics.FromImage(destinationImage);
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using var wrapMode = new ImageAttributes();
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, destinationRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

        return destinationImage;
    }

    private static Dimensions ImageDimensions(int width, int height, int maximumSizeInPixels)
    {
        var thumbnailWidth = width;
        var thumbnailHeight = height;

        if(width < maximumSizeInPixels || height < maximumSizeInPixels)
        {
            thumbnailWidth = width;
            thumbnailHeight = height;
        }
        else if(maximumSizeInPixels != 0)
        {
            thumbnailWidth = maximumSizeInPixels;
            thumbnailHeight = maximumSizeInPixels;
            if(width > height)
            {
                thumbnailHeight = SetProportionalDimension(width, height, maximumSizeInPixels);
            }
            else
            {
                thumbnailWidth = SetProportionalDimension(height, width, maximumSizeInPixels);
            }
        }

        return new() { Height = thumbnailHeight, Width = thumbnailWidth };
    }

    private static int SetProportionalDimension(int widthOrHeight, int heightOrWidth, int maximumThumbnailInPixels) => Convert.ToInt32(heightOrWidth * maximumThumbnailInPixels / (double)widthOrHeight);

    private static int RestrictMaximumSizeInPixels(int maximumSizeInPixels)
        => maximumSizeInPixels > MaximumHeightAndWidthForThumbnail
            ? MaximumHeightAndWidthForThumbnail
            : maximumSizeInPixels;

    private FileDetail? ReadDb(string directory, string filename)
    {
        try
        {
            return context.Files.FirstOrDefault(f => f.FileName == filename && f.DirectoryName == directory);
        }
        catch
        {
            _ = Task.Delay(TimeSpan.FromSeconds(2));
            return context.Files.FirstOrDefault(f => f.FileName == filename && f.DirectoryName == directory);
        }
    }
}
