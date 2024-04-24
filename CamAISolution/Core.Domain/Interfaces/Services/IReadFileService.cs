using Core.Domain.Enums;

namespace Core.Domain.Interfaces.Services;

public interface IReadFileService
{
    IEnumerable<T> ReadFromCsv<T>(Stream stream);
    T ReadFromJson<T>(Stream stream);
}