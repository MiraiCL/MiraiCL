using System.Net;
using System.Net.Sockets;
using MiraiCL.Core.Abstraction.Network;
using MiraiCL.Core.Logger;

namespace MiraiCL.Core.Network;

public class UdpTransport(string addr,ushort port):ISocketTransport{
        public Action<Stream>? OnClientConnectedCallback;

    private readonly CancellationTokenSource _cts = new();

    private readonly Socket _socket = new(SocketType.Dgram,ProtocolType.Udp);

    public IPEndPoint SocketEndPoint = IPEndPoint.Parse($"{addr}:{port}");

    public Func<Socket,Stream> WrapperStream = socket => new NetworkStream(socket,false);

    public bool EnableCompressSupport {get;set;} = false;

    public bool Connect(){
        try{
            _socket.Connect(SocketEndPoint);
        }catch(Exception ex){
            LogWrapper.Error(ex,"Network","连接到目标服务器失败");
        }
        return _socket.Connected;
    }

    public async Task<bool> ConnectAsync()
    {
        using var cts = new CancellationTokenSource(10000);
        try{
            await _socket.ConnectAsync(SocketEndPoint,cts.Token);
        }catch(TaskCanceledException){
            LogWrapper.Error("Network","连接到远程服务器失败：操作超时");
        }catch(Exception ex){
            LogWrapper.Error(ex,"Network","连接到远程服务器失败");
        }
        return _socket.Connected;

    }

    public void Disconnect(bool reusePort = false) => _socket.Disconnect(reusePort);

    public async Task DisconnectAsync(bool reusePort) => await _socket.DisconnectAsync(reusePort);

    public bool Bind(){
        try{
            _socket.Bind(SocketEndPoint);
        }catch(Exception ex){
            LogWrapper.Error(ex,"Network","绑定端口失败");
            return false;
        }
        return true;
    }

    

    public void SetForwardRule()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _cts.Cancel();
        _socket.Dispose();
        _cts.Dispose();
    }

    public async Task SendAsync(byte[] buffer,CancellationToken? token = null){
        if(!_socket.Connected) await _socket.SendToAsync(buffer,SocketEndPoint,token??CancellationToken.None);
        await _socket.SendAsync(buffer,token??CancellationToken.None);   
    }
}