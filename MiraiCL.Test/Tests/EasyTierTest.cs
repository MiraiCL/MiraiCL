using MiraiCL.Core.LinkLobby;
using MiraiCL.Core.Utils;

public class EasyTierTest{
    [SetUp]
    public static void ReleaseFFIFile(){
        Console.WriteLine(ResourceUtils.Names());
        using var fs = File.Open("./EasyTier.dll",FileMode.Create);
        ResourceUtils.GetEasyTier().CopyTo(fs);
    }
    [Test]
    public void Test(){
        var str = @"
hostname = ""123456""
instance_name = ""LuoTianyi""
instance_id = ""b46eff9b-ba53-45ee-b7fd-a2a2968a7c6b""
dhcp = true
listeners = [
    ""tcp://0.0.0.0:11010"",
    ""udp://0.0.0.0:11010"",
    ""wg://0.0.0.0:11011"",
]
rpc_portal = ""0.0.0.0:0""

[network_identity]
network_name = ""LuoTianyi""
network_secret = ""123456789""

[[peer]]
uri = ""tcp://public.easytier.top:11010""

[flags]
bind_device = false
enable_kcp_proxy = true
latency_first = true
private_mode = true
        ";
        EasyTierController.RunNetworkInstance(str);
    }
}