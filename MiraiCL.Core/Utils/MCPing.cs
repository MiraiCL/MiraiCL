using MiraiCL.Core.Abstraction.Network;
using MiraiCL.Core.Network;

namespace MiraiCL.Core.Utils;

public class MCPing(string addr,ushort port){
    private ISocketTransport? _transport;


    public MCPingResult? Ping()
    {
        return null;
    }
}