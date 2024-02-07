namespace Core.Domain.Utilities;

public static class FileHelper
{
    public static string GetExtension(string filename)
    {
        var index = filename.LastIndexOf('.');
        return index > -1
            ? filename[index..]
            : throw new ArgumentException($"Couldn't specify file's extension. Filename: {filename}");
    }

    public static void EnsureDirectoryExisted(string path)
    {
        Directory.CreateDirectory(path);
    }

    public static string ToPathString(this DateOnly date) => date.ToString("yyyyMMdd");

    public static string ToPathString(this DateTime date) => date.ToString("yyyyMMdd");
}
