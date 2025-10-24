using System.Reflection;
using System.Resources;

namespace MiraiCL.Core.Utils;

public class ResourceUtils{
    [Obsolete("此方法仅供测试用")]
    public static Stream? GetEasyTier() {
        var assembly = typeof(ResourceUtils).Assembly;
        return assembly.GetManifestResourceStream("MiraiCL.Core.Resources.EasyTierFFI.EasyTier.dll");
    }
    public static string Names(){
        var assembly = typeof(ResourceUtils).Assembly;
        var resourceNames = assembly.GetManifestResourceNames();
        
        var str = "";
        foreach (var name in resourceNames)
        {
            str += name + "\n";
        }
        return str;
    }
}