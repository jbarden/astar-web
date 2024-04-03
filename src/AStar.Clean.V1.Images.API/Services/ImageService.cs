using System.Drawing;
using System.Runtime.Versioning;

namespace AStar.Clean.V1.Images.API.Services;

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> logger;

    public ImageService(ILogger<ImageService> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    ///     This feels like it should be on the files API
    /// </summary>
    /// <param name="imagePath"></param>
    /// <returns></returns>
    [SupportedOSPlatform("windows")]
    public Image GetImage(string imagePath)
    {
        try
        {
            return Image.FromFile(imagePath);
        }
        catch(Exception e)
        {
            logger.LogError("An error occurred ({error}) whilst retrieving {fileName} - full stack: {stack}", e.Message,
                imagePath, e.StackTrace);
            var bmp = new Bitmap(100, 50);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.DarkRed);

            return bmp;
        }
    }
}