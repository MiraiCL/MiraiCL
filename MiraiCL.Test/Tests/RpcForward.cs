using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpProxy
{
    private readonly int _listenPort;
    private readonly string _targetHost;
    private readonly int _targetPort;
    
    public TcpProxy(int listenPort, string targetHost, int targetPort)
    {
        _listenPort = listenPort;
        _targetHost = targetHost;
        _targetPort = targetPort;
    }
    
    public async Task StartAsync()
    {
        var listener = new TcpListener(IPAddress.Any, _listenPort);
        listener.Start();
        
        Console.WriteLine($"代理服务器启动，监听端口: {_listenPort}");
        Console.WriteLine($"目标服务器: {_targetHost}:{_targetPort}");
        Console.WriteLine("按 Ctrl+C 停止服务\n");
        
        try
        {
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client); // 不等待，异步处理每个客户端
            }
        }
        finally
        {
            listener.Stop();
        }
    }
    
    private async Task HandleClientAsync(TcpClient client)
    {
        TcpClient targetClient = null;
        
        try
        {
            using (client)
            {
                // 连接到目标服务器
                targetClient = new TcpClient();
                await targetClient.ConnectAsync(_targetHost, _targetPort);
                
                using (targetClient)
                using (var clientStream = client.GetStream())
                using (var targetStream = targetClient.GetStream())
                {
                    // 创建双向数据转发任务
                    var clientToTarget = ForwardDataAsync(
                        clientStream, targetStream, 
                        $"客户端 -> 目标服务器({_targetHost}:{_targetPort})");
                    
                    var targetToClient = ForwardDataAsync(
                        targetStream, clientStream, 
                        $"目标服务器({_targetHost}:{_targetPort}) -> 客户端");
                    
                    // 等待任意一个方向的数据传输完成
                    await Task.WhenAny(clientToTarget, targetToClient);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"处理客户端连接时出错: {ex.Message}");
        }
        finally
        {
            targetClient?.Close();
        }
    }
    
    private async Task ForwardDataAsync(NetworkStream source, NetworkStream destination, string direction)
    {
        byte[] buffer = new byte[4096];
        
        try
        {
            while (true)
            {
                // 读取数据
                var bytesRead = await source.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                    break; // 连接关闭
                
                // 打印请求数据（如果是字符串）
                string dataString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"=== {direction} ===");
                Console.WriteLine($"数据长度: {bytesRead} 字节");
                Console.WriteLine($"内容: {dataString}");
                Console.WriteLine($"十六进制: {BitConverter.ToString(buffer, 0, bytesRead)}");
                Console.WriteLine("=== 结束 ===\n");
                
                // 转发数据到目标
                await destination.WriteAsync(buffer, 0, bytesRead);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"数据传输错误 ({direction}): {ex.Message}");
        }
    }
}

// 使用示例
class Program
{
    public void Main()
    {
        // 配置参数
        int listenPort = 10000;           // 监听的端口
        string targetHost = "localhost";  // 目标服务器地址
        int targetPort = 10024;           // 目标服务器端口
        
        var proxy = new TcpProxy(listenPort, targetHost, targetPort);
        
        try
        {
            proxy.StartAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"服务器错误: {ex.Message}");
        }
    }
}