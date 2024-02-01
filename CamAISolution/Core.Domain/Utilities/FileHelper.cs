namespace Core.Domain.Utilities;

public static class FileHelper
{
    public static string GetExtension(string filename)
    {
        return filename.Substring(filename.LastIndexOf('.'));
    }   
}
