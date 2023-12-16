using Core.Domain;
using Infrastructure.Logging;

namespace Test.Infrastructure.Logging;

public class LoggingTest
{
    private IAppLogging<LoggingTest> logger;
    [OneTimeSetUp]
    public void Setup()
    {
        logger = new AppLogging<LoggingTest>();
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
        logger.Warm("Test");
        Assert.Pass();
    }

    [Test]
    public void Log_error()
    {
        logger.Error("Test", new Exception("Error"));
        Assert.Pass();
    }

}