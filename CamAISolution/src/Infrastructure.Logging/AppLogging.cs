using Core.Domain;
using Serilog;
using Serilog.Core;

namespace Infrastructure.Logging;

public class AppLogging<T> : IAppLogging<T>
{
    private readonly Logger log;

    public AppLogging()
    {
        log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }

    private Func<string, string> MessageTemplate = msg => $"[{nameof(T)}]: {msg}";
    public void Error(string message, Exception? exception)
    {
        log.Error(exception, MessageTemplate(message));
    }

    public Task ErrorAsync(string message, Exception? exception)
    {
        throw new NotImplementedException();
    }

    public void Info(string message)
    {
        log.Information(MessageTemplate(message));
    }

    public Task InfoAsync(string message)
    {
        throw new NotImplementedException();
    }

    public void Warm(string message)
    {
        log.Warning(MessageTemplate(message));
    }

    public Task WarmAsync(string message)
    {
        throw new NotImplementedException();
    }
}
