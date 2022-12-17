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

    public static string CreateGif(string gifFilename, IEnumerable<string> frameImages, Fps fps = Fps.Fps30, int repeat = 0)
    {
        using var gif = AnimatedGif.AnimatedGif.Create(gifFilename, (int)fps, repeat);

        foreach (var frameImage in frameImages)
        {
            var img = Image.FromFile(frameImage);
            gif.AddFrame(img, delay: -1, quality: GifQuality.Bit8);
        }

        return gifFilename;
    }

    public static string CreateGif(string gifFilename, IEnumerable<Image> frameImages, Fps fps = Fps.Fps30, int repeat = 0)
    {
        using var gif = AnimatedGif.AnimatedGif.Create(gifFilename, (int)fps, repeat);

        foreach (var img in frameImages)
        {
            gif.AddFrame(img, delay: -1, quality: GifQuality.Bit8);
        }

        return gifFilename;
    }
}