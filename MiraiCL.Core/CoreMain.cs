using MiraiCL.Core.App;
using MiraiCL.Core.Logger;

namespace MiraiCL.Core;

public class CoreMain{
    internal static Logger.Logger logger = new(new LogOptions(){
        LogBasePath = AppPath.LogPath,
        DisposeTimeout = 10,
        AutoDelete = true,
        LogLevel = Information.Channel == ReleaseChannel.Develop ? LogLevel.Debug:LogLevel.Trace,
        MaxSize = 25*1024*1024,
        OutdateTime = 7,
        Rule = Information.Channel == ReleaseChannel.Develop ? ConsoleRule.Default:ConsoleRule.Ignore
    });
    public static void MainEntryPoint(string[] arguments){

    }
}