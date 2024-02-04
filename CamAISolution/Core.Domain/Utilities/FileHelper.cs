namespace Core.Domain.Utilities;

public static class FileHelper
{
    public static string GetExtension(string filename)
    {
        var index = filename.LastIndexOf('.');
        return index > -1
            ? filename.Substring(index)
            : throw new ArgumentException($"Couldn't specify file's extension. Filename: {filename}");
    }
}
