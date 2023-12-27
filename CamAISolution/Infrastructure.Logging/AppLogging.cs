using Core.Domain;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging;

public class AppLogging<T>(ILoggerFactory loggerFactory) : IAppLogging<T>
{
    private readonly ILogger logger = loggerFactory.CreateLogger($"{typeof(T).FullName}");

    private readonly Func<string, string> MessageTemplate = msg => $"[{typeof(T).Name}]: {msg}";

    public void Error(string message, Exception? exception)
    {
        logger.LogError(exception, MessageTemplate(message));
    }

    public Task ErrorAsync(string message, Exception? exception)
    {
        throw new NotImplementedException();
    }

    public void Info(string message)
    {
        logger.LogInformation(MessageTemplate(message));
    }

    public Task InfoAsync(string message)
    {
        throw new NotImplementedException();
    }

    public void Warn(string message)
    {
        logger.LogWarning(MessageTemplate(message));
    }

    public Task WarnAsync(string message)
    {
        throw new NotImplementedException();
    }
}
