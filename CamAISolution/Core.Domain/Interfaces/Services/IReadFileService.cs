using Core.Domain.Enums;

namespace Core.Domain.Interfaces.Services;

public interface IReadFileService
{
    IEnumerable<T> ReadFile<T>(Stream stream, FileType type);
}