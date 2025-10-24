using System.Diagnostics;
using MiraiCL.Core.Abstraction.Network;
using MiraiCL.Core.Services.Logger;

namespace MiraiCL.Core.Network;

public class DownloadItem:DownloadItemBase
{
    public const int AllowMiniumSize = 1024*1024*5;
    public DownloadItemBase? ParentItem;

    public bool IsComplete { get; set; }

    public long DownloadedSize
    {
        get => _downloadSize;
        set
        {
            
        }
    }

    public int DownloadSpeed;

    public CancellationTokenSource Cts = new();

    private long _downloadSize;

    public void Cancel() => Cts.Cancel();

    public void UpdateSpeed()
    {
        Task.Run(async () =>
        {
            while (!Cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                DownloadSpeed = 0;
            }
        });
    }
    
    // 256 KiB/s
    public int AllowMiniumSpeed = 256 * 1024;
    public override async Task StartDownloadAsync(TimeSpan timeout)
    {
        var logger = LogService.CurrentLogger;
        UpdateSpeed();
        var watcher = new Stopwatch();
        watcher.Start();
        foreach (var source in Sources)
        {
            try
            {
                logger?.Info("Network",$"下载文件 {source}，起始点：{StartedSize}，结束点：{EndedSize}");
                using var cts = new CancellationTokenSource(timeout);
                using var response = await WebUtils.GetContentWithSize(source, StartedSize, EndedSize, cts.Token);
                await using var fs = new FileStream(
                    Path,
                    FileMode.CreateNew,
                    FileAccess.ReadWrite,
                    FileShare.None, 65535, true);
                var buffer = new byte[32768];
                await using var responseStream = await response.Content.ReadAsStreamAsync(cts.Token);
                cts.CancelAfter(TimeSpan.FromSeconds(16));
                while (responseStream.CanRead)
                {
                    var readCount = await responseStream.ReadAsync(buffer, cts.Token);
                    if (readCount == 0) break;
                    DownloadedSize += readCount;
                    await fs.WriteAsync(buffer, cts.Token);
                    if (DownloadSpeed < AllowMiniumSpeed) throw new TimeoutException("");
                    Array.Clear(buffer, 0, buffer.Length);
                }
                logger?.Info("Network",$"下载文件 {source} 完成，用时 {watcher.ElapsedMilliseconds/1000}s，平均速度：");
            }
            catch (TaskCanceledException ex)
            {
                // Operation Canceled
                if (Cts.IsCancellationRequested) return;
            }


        }
    }

    private static readonly string[] _speedTexts = ["Byte/s", "KiB/s", "MiB/s", "GiB/s", "TiB/s"];
    public static string GetReadableSpeed(double speed)
    {
        if (speed <= 0) return "0 B/s";
    
        int unitIndex = (int)Math.Floor(Math.Log(speed, 1024));
        unitIndex = Math.Min(unitIndex, 4); // 限制最大单位为TiB/s
    
        double readableSpeed = speed / Math.Pow(1024, unitIndex);
        string format = readableSpeed >= 100 ? "F0" : readableSpeed >= 10 ? "F1" : "F2";
    
        return $"{readableSpeed.ToString(format)}{_speedTexts[unitIndex]}";
    }

    public override List<IDownloadItem> Split(int threadCount)
    {
        if (TotalFileSize < AllowMiniumSize) 
            return [this];
    
        var chunkSize = Math.Max(AllowMiniumSize, (long)Math.Ceiling((double)TotalFileSize / threadCount));
        var chunkCount = (int)Math.Ceiling((double)TotalFileSize / chunkSize);
        var items = new List<IDownloadItem>();
    
        for (var i = 0; i < chunkCount; i++)
        {
            var startedSize = i * chunkSize;
            var endedSize = Math.Min(startedSize + chunkSize - 1, TotalFileSize - 1);
            var chunkTotalSize = endedSize - startedSize + 1;
        
            var item = new DownloadItem()
            {
                StartedSize = startedSize,
                EndedSize = endedSize,
                TotalFileSize = chunkTotalSize
            };
        
            items.Add(item);
        }
    
        return items;
    }
}