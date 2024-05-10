namespace Core.Domain.Interfaces.Services;

public interface IReadFileService
{
    IEnumerable<T> ReadFromCsv<T>(Stream stream, bool isCacheCountRecords = false, string cachedKey = "");
    T ReadFromJson<T>(Stream stream);
}