using System.Net;
using System.Net.Sockets;
using MiraiCL.Core.Network;

namespace MiraiCL.Core.LinkLobby;

public class LobbyClient:IDisposable{
    private LobbyCode? _code;
    private TcpTransport _transport;
    private CancellationTokenSource _cts = new();
    ///<summary>
    /// Experimental: Enable transport optimization on the current network to reduce bandwidth usage. <br/>
    /// Please note it may cause connection issue.
    ///</summary>
    public bool EnableTransportOptimization {get;set;} = false;

    public bool EnableSocks5ForwardSupport {get;set;} = true;

    private string? _targetHost;

    private ushort? _targetPort;

    public LobbyClient(string lobbyCode){
        _code = LobbyCode.Parse(lobbyCode);
    }

    public void StartForward(){
        Task.Run(_ForwardDataPacket);
    }

    private async Task _ForwardDataPacket(){
        while(!_cts.IsCancellationRequested){
            
        }
    }

    public async Task Handshake(){
        await _transport.Connect();
    }    

    public void Dispose()
    {
        _cts.Cancel();
    }
}