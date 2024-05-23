namespace Core.Domain.Interfaces.Services;

public interface IReadFileService
{
    IEnumerable<T> ReadFromCsv<T>(Stream stream, string cachedFaledRecordsKey, bool isCacheCountRecords = false, string cachedProgressKey = "");
    T ReadFromJson<T>(Stream stream);
}