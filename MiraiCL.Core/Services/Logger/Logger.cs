using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MiraiCL.Core.Services.Logger;

public class Logger(LogOptions options) : IDisposable
{
    private LogOptions _option = options;
    private bool _disposed;
    private FileStream? _logStream;
    private readonly object _lock = new object();
    public CancellationTokenSource Cts = new();
    public FileStream? LogStream
    {
        get 
        {
            lock (_lock)
            {
                return _logStream ??=
                    new FileStream(LogPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 16384, true);
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

    public string LogPath
    {
        get 
        {
            for(var i = 0;i<16;i++)
            {
                _logPath ??= Path.Combine(UPath.LogPath,
                        $"{DateTime.Now.Date}-{Random.Shared.Next()}.log");
                if (!File.Exists(_logPath)) return _logPath;
            }

            throw new OperationCanceledException("Cloud not found a ");

        }
        set => _logPath = value;
    }

    private void _Log(string message,bool isErr = false)
    {
        if (_disposed) throw new InvalidOperationException("This logger already disposed.");
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

    private void _Log(LogLevel level, string module, string message)
    {
        _Log($"[{level}] | [{module}] : {message}",level > LogLevel.Warning);
    }

    private string _GetInvokeStack(Exception ex) => ex.ToString();


    private void _StartLogThread()
    {
        Task.Run(async () =>
        {
            await foreach (var logString in LogChannel.Reader.ReadAllAsync(Cts.Token))
            {
                await LogStream!.WriteAsync(Encoding.UTF8.GetBytes(logString));
            }
        });
    }

    private void _Log(LogLevel level, Exception ex, string module, string message)
    {
        _Log(level, module, $"{message}:{_GetInvokeStack(ex)}");
    }

    public void Trace(string message)
    {
        
    }
    public void Trace(Exception? ex,string message)
    {
        
    }
    public async Task DisposeAsync()
    {
        _disposed = true;
        Cts.CancelAfter(TimeSpan.FromSeconds(10));
        if (LogStream is not null) await LogStream.DisposeAsync();
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

    public DateTime OutdateTime { get; set; }
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