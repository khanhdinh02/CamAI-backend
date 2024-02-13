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

    public static void EnsureDirectoryExisted(string path) => Directory.CreateDirectory(path);

    public static string ToFilePath(this DateOnly date) =>
        Path.Combine(date.Year.ToString(), date.Month.ToString(), date.Day.ToString());

    public static string ToFilePath(this DateTime date) => ToFilePath(DateOnly.FromDateTime(date));

    public static string ToDirPath(this DateOnly date) => Path.Combine(date.Year.ToString(), date.Month.ToString());

    public static string ToDirPath(this DateTime date) => ToDirPath(DateOnly.FromDateTime(date));
}
