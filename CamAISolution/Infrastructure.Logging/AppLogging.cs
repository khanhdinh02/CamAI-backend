using Core.Domain;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging;

public class AppLogging<T>(ILoggerFactory loggerFactory) : IAppLogging<T>
{
    private readonly ILogger logger = loggerFactory.CreateLogger($"{typeof(T).FullName}");

    public void Error(string message, Exception? exception = null, params object?[] args) =>
        logger.LogError(exception, message, args);

    public void Info(string message, params object?[] args) => logger.LogInformation(message, args);

    public void Warn(string message, params object?[] args) => logger.LogWarning(message, args);
}
