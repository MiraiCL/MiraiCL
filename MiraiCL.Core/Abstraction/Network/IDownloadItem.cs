namespace MiraiCL.Core.Abstraction.Network;

public interface IDownloadItem
{
    Task StartDownloadAsync(TimeSpan timeout);
    
    List<IDownloadItem> Split(int threadCount);
}