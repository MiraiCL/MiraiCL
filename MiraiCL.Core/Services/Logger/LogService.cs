using MiraiCL.Core.Services.ServiceManager;

namespace MiraiCL.Core.Services.Logger;

public class LogService:Service
{
    public static Logger? CurrentLogger;

    public override void Startup()
    {
        CurrentLogger = new Logger(new LogOptions
        {
            AutoDelete = true, DisposeTimeout = 10, LogBasePath = "./app", LogLevel = LogLevel.Trace,
            MaxSize = 6535 * 65535, OutdateTime = 1625656, Rule = ConsoleRule.Default
        });
    }

    public override void Shutdown() => CurrentLogger?.Dispose();
}