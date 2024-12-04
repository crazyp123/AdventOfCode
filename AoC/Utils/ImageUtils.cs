using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AnimatedGif;

namespace AoC.Utils;

public static class ImageUtils
{
    public enum Fps
    {
        Fps30 = 33,
        Fps60 = 16
    }

    public static string CreateGif(string gifFilename, IEnumerable<string> frameImages, Fps fps = Fps.Fps30,
        int repeat = 0)
    {
        using var gif = AnimatedGif.AnimatedGif.Create(gifFilename, (int)fps, repeat);

        foreach (var frameImage in frameImages)
        {
            var img = Image.FromFile(frameImage);
            gif.AddFrame(img, -1, GifQuality.Bit8);
        }

        return gifFilename;
    }

    public static string CreateGif(string gifFilename, IEnumerable<Image> frameImages, Fps fps = Fps.Fps30,
        int repeat = 0)
    {
        using var gif = AnimatedGif.AnimatedGif.Create(gifFilename, (int)fps, repeat);

        foreach (var img in frameImages) gif.AddFrame(img, -1, GifQuality.Bit8);

        return gifFilename;
    }

    public static Image ScaleUp(this Image originalImage, int scale)
    {
        var newWidth = originalImage.Width * scale;
        var newHeight = originalImage.Height * scale;
        var scaledImage = new Bitmap(newWidth, newHeight);

        using (var graphics = Graphics.FromImage(scaledImage))
        {
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graphics.DrawImage(originalImage, new Rectangle(0, 0, newWidth, newHeight));
        }

        return scaledImage;
    }
}