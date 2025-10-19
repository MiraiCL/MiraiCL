namespace MiraiCL.Core.Abstraction.Network;

public abstract class DownloadItemBase:IDownloadItem
{
    public string[] Sources { get; set; }

    public string Path { get; set; }

    public long StartedSize { get; set; } = 0;
    public long EndedSize { get; set; } = 0;
    
    
    public long TotalFileSize { get; set; }

    public Func<DownloadItemBase,bool> CheckIfValid { get; set; }

    public Dictionary<string, Exception> ErrorPerSource;

    public abstract Task StartDownloadAsync(TimeSpan timeout);

    public abstract List<IDownloadItem> Split(int threadCount);
}