using Core.Application.Exceptions;
using SkiaSharp;

namespace Infrastructure.Blob;

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
        InitiateSkImage(out var bitmap, out var imageInfo, physicalPath, null, scaleFactor, width, height);
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

    /// <summary>
    /// Try to reduce size as much as possible
    /// </summary>
    public static byte[] TryCompressImage(byte[] imageBytes)
    {
        InitiateSkImage(out var bitmap, out var imageInfo, null, imageBytes, 1f);
        using var resizedBitmap = bitmap.Resize(imageInfo, SKFilterQuality.Low);
        using var image = SKImage.FromBitmap(resizedBitmap);
        int quality = 100;
        var encodedImage = image.Encode(SKEncodedImageFormat.Jpeg, quality);
        while (quality > 1)
        {
            if (encodedImage.AsStream().Length <= 1024 * 100) // 100KB
                break;
            quality -= 3;
            encodedImage = image.Encode(SKEncodedImageFormat.Jpeg, quality);
            if (quality == 1 && encodedImage.Size > 1024 * 100)
            {
                encodedImage = DownScale(encodedImage, bitmap);
            }
        }

        using var memoryStream = new MemoryStream();
        encodedImage.AsStream().CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

    private static SKData DownScale(SKData data, SKBitmap bitmap)
    {
        float scaleFactor = 1;
        while (scaleFactor > 0.3)
        {
            if (data.Size <= 1024 * 100)
                return data;
            float newWidth = bitmap.Width * scaleFactor;
            float newHeight = bitmap.Height * scaleFactor;
            scaleFactor -= 0.1f;
            var imageInfo = new SKImageInfo((int)newWidth, (int)newHeight);
            using var resizedBitmap = bitmap.Resize(imageInfo, SKFilterQuality.Low);
            using var image = SKImage.FromBitmap(resizedBitmap);
            data = image.Encode();
        }
        return data;
    }

    private static void InitiateSkImage(
        out SKBitmap bitmap,
        out SKImageInfo imageInfo,
        string? physicalPath,
        byte[]? imageBytes,
        float scaleFactor,
        int? width = null,
        int? height = null
    )
    {
        SKImage skImage;
        if (physicalPath != null)
            skImage = SKImage.FromEncodedData(physicalPath);
        else if (imageBytes != null)
            skImage = SKImage.FromEncodedData(imageBytes);
        else
            throw new ArgumentException("Physical path of image or image bytes is not provided");
        bitmap = SKBitmap.FromImage(skImage);
        float newWidth = width.HasValue ? width.Value * scaleFactor : bitmap.Width * scaleFactor;
        float newHeight = height.HasValue ? height.Value * scaleFactor : bitmap.Height * scaleFactor;
        imageInfo = new SKImageInfo((int)newWidth, (int)newHeight);
    }
}
