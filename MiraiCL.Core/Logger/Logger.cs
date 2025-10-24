using System.Text;
using System.Threading.Channels;

namespace MiraiCL.Core.Logger;

public class Logger(LogOptions options) : IDisposable
{
    /// <summary>
    /// Invoke when write file failure <br/> For receiving exception only.
    /// </summary>
    public event Action<Exception>? OnLogFailed;
    /// <summary>
    /// Invoke when write to file <br/> For receiving log information only.
    /// </summary>
    public event Action<string>? OnLog;

    private LogOptions _option = options;
    private bool _disposed;
    private FileStream? _logStream;
    private readonly object _lock = new object();
    public CancellationTokenSource Cts = new();
    private bool _running;

    public FileStream? LogStream
    {
        get 
        {
            lock (_lock)
            {
                return _logStream ??=
                    new FileStream(LogPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 1048576, true);
            }
        }

    
        set => _ = value;
    }

    public Channel<string> LogChannel
    {
        get => _logChannel ??= Channel.CreateUnbounded<string>();
    }

    private Channel<string>? _logChannel;

    private string? _logPath;

    private int _writedLogsize;

    public string LogPath
    {
        get 
        {
            if(!string.IsNullOrEmpty(_logPath)) return _logPath;
            for(var i = 0;i<16;i++)
            {
                _logPath = Path.Combine(_option.LogBasePath,
                        $"{DateTime.Now.Date:yyyy-MM-dd}-{Random.Shared.Next()}.log");
                


                if (File.Exists(_logPath)) continue;
                var dir = Path.GetDirectoryName(_logPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return _logPath;
            }

            throw new OperationCanceledException("Cloud not found a valid path.");

        }
        set => _logPath = value;
    }

    private static string _GetInvokeStack(Exception ex) => ex.ToString();

    private void _DeleteOutdateLog(){
        if(!Directory.Exists(_option.LogBasePath)) return;
        foreach(var logFile in Directory.EnumerateFiles(_option.LogBasePath)){
            if ((DateTime.Now - new FileInfo(logFile).CreationTime).TotalDays > _option.OutdateTime){
                try{
                    File.Delete(logFile);
                }
                catch
                {
                    // ignored
                }
            }
        }
    }

    private void _StartLogThread()
    {
        _running = true;
        _DeleteOutdateLog();
        Task.Run(async () =>
        {
            await foreach (var logString in LogChannel.Reader.ReadAllAsync(Cts.Token))
            {
                try{
                    OnLog?.Invoke(logString);
                    var logByte = Encoding.UTF8.GetBytes(logString + Environment.NewLine);
                    await LogStream!.WriteAsync(logByte);
                    _writedLogsize += logByte.Length;
                    if(_option.MaxSize > 0 && _writedLogsize > _option.MaxSize){
                        await LogStream.DisposeAsync();
                        _logPath = null;
                        _logStream = null;
                        _writedLogsize = 0;
                    }
                }catch(Exception ex){
                    Error(ex,"Logger","9");
                    OnLogFailed?.Invoke(ex);
                }
            }
        });
    }

    private void _Log(string message,bool isErr = false)
    {
        message = $"{DateTime.Now.ToString("HH:mm:ss.sss")} | {message}";

        if (_disposed) throw new InvalidOperationException("This logger already disposed.");
        lock(_lock){
            if(!_running && !_disposed) _StartLogThread();
        }
        switch (_option.Rule)
        {
            case ConsoleRule.Default:
                if (isErr) Console.Error.WriteLine(message);
                else Console.WriteLine(message);
                break;
            case ConsoleRule.RedirectStdOutToStdErr:
                Console.Error.WriteLine(message);
                break;
            case ConsoleRule.RedirectStdErrToStdOut:
                Console.WriteLine(message);
                break;
            //skip ConsoleRule.Ignore (Write to file only)
        }
        LogChannel.Writer.TryWrite(message);
    }

    internal void _Log(LogLevel level, string module, string message)
    {
        if (level < _option.LogLevel) return;
        _Log($"[{level}] | [{module}] : {message}",level > LogLevel.Warning);
    }

    internal void _Log(LogLevel level, Exception ex, string module, string message)
    {
        _Log(level, module, $"{message}:{_GetInvokeStack(ex)}");
    }

    public void Trace(string module,string message) => _Log(LogLevel.Trace,module,message);
    public void Trace(Exception ex,string module,string message) => _Log(LogLevel.Trace,ex,module,message);
    public void Debug(string module,string message) => _Log(LogLevel.Debug,module,message);
    public void Debug(Exception ex,string module,string message) => _Log(LogLevel.Debug,ex,module,message);
    public void Info(string module,string message) => _Log(LogLevel.Info,module,message);
    public void Info(Exception ex,string module,string message) => _Log(LogLevel.Info,ex,module,message);
    public void Warning(string module,string message) => _Log(LogLevel.Warning,module,message);
    public void Warning(Exception ex,string module,string message) => _Log(LogLevel.Warning,ex,module,message);
    public void Error(string module,string message) => _Log(LogLevel.Error,module,message);
    public void Error(Exception ex,string module,string message) => _Log(LogLevel.Error,ex,module,message);

    public async Task DisposeAsync()
    {
        // Maybe disposed in other thread
        if(_disposed) return;
        _disposed = true;
        Cts.CancelAfter(TimeSpan.FromSeconds(_option.DisposeTimeout));
        LogChannel.Writer.Complete();
        await Task.Delay(TimeSpan.FromSeconds(_option.DisposeTimeout));
        if (LogStream is not null) await LogStream.DisposeAsync();
        Cts.Dispose();
    }
    /// <summary>
    /// Flush all log and dispose <see cref="FileStream"/>
    /// </summary>
    public void Dispose()
    {
        Task.Run(DisposeAsync).GetAwaiter().GetResult();
    }
}

public class LogOptions
{
    /// <summary>
    /// Current log level. (when log level lower than this will be discard.)
    /// </summary>
    public LogLevel LogLevel { get; set; } = LogLevel.Info;
    /// <summary>
    /// Set console output rule
    /// </summary>
    public ConsoleRule Rule { get; set; } = ConsoleRule.Default;
    /// <summary>
    /// Max size per file (Do not rotate files when the value is zero.)
    /// </summary>
    public long MaxSize { get; set; }
    /// <summary>
    /// Automatic delete outdate log. (Must set OutdateTime property)
    /// </summary>
    public bool AutoDelete  {get; set; }

    public int OutdateTime { get; set; }

    public required string LogBasePath {get;set;}

    public int DisposeTimeout {get;set;} = 10;
}

public enum ConsoleRule
{
    /// <summary>
    /// Standard Error -> Standard Output
    /// </summary>
    RedirectStdErrToStdOut,
    /// <summary>
    /// Standard Output -> Standard Error
    /// </summary>
    RedirectStdOutToStdErr,
    /// <summary>
    /// Isolated output
    /// </summary>
    Default,
    /// <summary>
    /// Do not output (File only)
    /// </summary>
    Ignore
}

public enum LogLevel
{
    /// <summary>
    /// Level = 0
    /// </summary>
    Trace,
    /// <summary>
    /// Level = 1
    /// </summary>
    Debug,
    /// <summary>
    /// Level = 2
    /// </summary>
    Info,
    /// <summary>
    /// Level = 3
    /// </summary>
    Warning,
    // Level = 4
    Error
}