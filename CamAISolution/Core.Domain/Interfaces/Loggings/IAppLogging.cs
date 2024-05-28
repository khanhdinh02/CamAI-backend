namespace Core.Domain;

public interface IAppLogging<T>
{
    void Info(string message, params object?[] args);
    void Warn(string message, params object?[] args);
    void Error(string message, Exception? exception = null, params object?[] args);
}
