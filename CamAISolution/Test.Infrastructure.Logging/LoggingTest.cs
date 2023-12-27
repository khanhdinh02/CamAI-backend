using Core.Domain;
using Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace Test.Infrastructure.Logging;

public class LoggingTest
{
    private IAppLogging<LoggingTest> logger;

    [OneTimeSetUp]
    public void Setup()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });
        logger = new AppLogging<LoggingTest>(loggerFactory);
    }

    [Test]
    public void Log_info()
    {
        logger.Info("Test");
        Assert.Pass();
    }

    [Test]
    public void Log_warm()
    {
        logger.Warn("Test");
        Assert.Pass();
    }

    [Test]
    public void Log_error()
    {
        logger.Error("Test", new Exception("Error"));
        Assert.Pass();
    }
}
