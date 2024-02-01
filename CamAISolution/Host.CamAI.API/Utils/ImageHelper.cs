using Core.Application.Exceptions;
using SkiaSharp;

namespace Host.CamAI.API.Utils;

public static class ImageHelper
{
    private static readonly Dictionary<string, SKEncodedImageFormat> _skiaSharpImageFormatMapping =
        new(StringComparer.InvariantCultureIgnoreCase)
        {
            { ".png", SKEncodedImageFormat.Png },
            { ".jpg", SKEncodedImageFormat.Jpeg },
            { ".jpeg", SKEncodedImageFormat.Jpeg },
            { ".jpe", SKEncodedImageFormat.Jpeg }
        };

    public static Stream Resize(string physicalPath, int? width, int? height, float scaleFactor = 1)
    {
        var imgSkia = SKImage.FromEncodedData(physicalPath);
        var bitmap = SKBitmap.FromImage(imgSkia);
        float newWidth = width.HasValue ? width.Value * scaleFactor : bitmap.Width * scaleFactor;
        float newHeight = height.HasValue ? height.Value * scaleFactor : bitmap.Height * scaleFactor;
        var imageInfo = new SKImageInfo((int)newWidth, (int)newHeight);
        using var resizedBitmap = bitmap.Resize(imageInfo, SKFilterQuality.Low);
        using var image = SKImage.FromBitmap(resizedBitmap);
        if (
            !_skiaSharpImageFormatMapping.TryGetValue(
                Core.Domain.Utilities.FileHelper.GetExtension(physicalPath),
                out var format
            )
        )
            throw new ServiceUnavailableException("Cannot resolve format of image");
        return image.Encode(format, 50).AsStream();
    }
}
