using MiraiCL.Core.Abstraction.Network;
using MiraiCL.Core.Network;

namespace MiraiCL.Core.Utils;

public class MCPing{
    private ISocketTransport _transport;

    public MCPing(string addr,ushort port){
        _transport = new TcpTransport(addr, port);
    }

    public 
}