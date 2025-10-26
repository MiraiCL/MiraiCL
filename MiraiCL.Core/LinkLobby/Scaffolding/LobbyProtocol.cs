using System.Text;
using MiraiCL.Core.Exts;

namespace MiraiCL.Core.LinkLobby.Scaffolding;

public enum ScaffoldingCommand{
    Ping,
    Protocols,
    ServerPort,
    PlayerPing,
    PlayerProfileList,
    Unknown
}
// ReSharper disable All
public enum MiraiCLCommand{
    KickPlayer,
    VoiceChatInviteRequest,
    ServicePort,
    Mute,
    EncryptConnectionRequest,
}
// ReSharper restore All


public class LobbyProtocol{

    private static readonly List<string> ProtocolList = [
        "c:ping",
        "c:protocols",
        "c:server_port",
        "c:player_ping",
        "c:player_profiles_list",
        "miraicl:encrypt_connection_request",
        "miraicl:service_port",
        "miraicl:voice_chat_invite_request",
        "miraicl:kick_player",
        "miraicl:mute_player"
    ];

    public required Enum Command;

    public static LobbyProtocol Parse(byte[] data){
        var i = 0;
        var methodLength = (int)data[i];
        i++;
        var method = data[i..methodLength];
        
        var protocol = new LobbyProtocol{
            Command = Encoding.UTF8.GetString(method) switch{
                "miraicl:encrypt_connection_request" => MiraiCLCommand.EncryptConnectionRequest,
                _ => ScaffoldingCommand.Unknown
            }
        };

        return protocol;
    }

    public static byte[] GetPing(){
        var pingByte = "c:ping".GetBytes();
        var content = "LuoTianyi&YueZhengling".GetBytes();
        var command = new byte[pingByte.Length+content.Length+1];
        command[0] = (byte)pingByte.Length;
        for(var i = 1;i<pingByte.Length+1;i++){
            command[i] = pingByte[i];
        }
        for(var i = 1+pingByte.Length;i<content.Length+1+pingByte.Length;i++){
            command[i] = content[i-1-pingByte.Length];
        }
        return command;
    }

    public static byte[] GetProtocols(){
        
        var protocolByte = "c:protocols".GetBytes();
        var payload = string.Join("\0",ProtocolList).GetBytes();
        var command = new byte[protocolByte.Length+payload.Length+1];
        command[0] = (byte)protocolByte.Length;
        for(var i = 1;i < protocolByte.Length +1;i++){
            command[i] = protocolByte[i];
        }
        for(var i = 1 + protocolByte.Length;i< 1 +protocolByte.Length + payload.Length;i++){
            command[i] = payload[i-protocolByte.Length-1];
        }
        return command;
    }
}