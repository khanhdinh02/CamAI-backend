namespace Core.Domain;

public interface IAppLogging<T>
{
    void Info(string message);
    void Warn(string message);
    void Error(string message, Exception? exception = null);

    //Async mehotd. Ex: writing log to file without blocking another proccesses 
    Task InfoAsync(string message);
    Task WarnAsync(string message);
    Task ErrorAsync(string message, Exception? exception);
}
