namespace MiraiCL.Core.Logger;

public static class LogWrapper{
    public static void Trace(string module,string message) => CoreMain.logger._Log(LogLevel.Trace,module,message);
    public static void Trace(Exception ex,string module,string message) => CoreMain.logger._Log(LogLevel.Trace,ex,module,message);
    public static void Debug(string module,string message) => CoreMain.logger._Log(LogLevel.Debug,module,message);
    public static void Debug(Exception ex,string module,string message) => CoreMain.logger._Log(LogLevel.Debug,ex,module,message);
    public static void Info(string module,string message) => CoreMain.logger._Log(LogLevel.Info,module,message);
    public static void Info(Exception ex,string module,string message) => CoreMain.logger._Log(LogLevel.Info,ex,module,message);
    public static void Warning(string module,string message) => CoreMain.logger._Log(LogLevel.Warning,module,message);
    public static void Warning(Exception ex,string module,string message) => CoreMain.logger._Log(LogLevel.Warning,ex,module,message);
    public static void Error(string module,string message) => CoreMain.logger._Log(LogLevel.Error,module,message);
    public static void Error(Exception ex,string module,string message) => CoreMain.logger._Log(LogLevel.Error,ex,module,message);
}

