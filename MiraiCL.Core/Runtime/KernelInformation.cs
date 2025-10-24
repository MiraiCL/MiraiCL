#define WINDOWS

#if WINDOWS
using System.Management;
#endif
using System.IO;
using System.Runtime.InteropServices;

namespace MiraiCL.Core.Runtime;

public static class KernelInformation{

    #if WINDOWS
    [DllImport("ntdll.dll")]
    private static extern void RtlGetNtVersionNumbers(out int major,out int minnor,out int build);    
    
    private static string Win32GetCurrentProcessId(){
        var management = new ManagementClass("Win32_Processer");
        var collection = management.GetInstances();
        var cpuId = string.Empty;
        foreach(var obj in collection){
            var id = obj.Properties["ProcessorId"];
            if(id is not null) cpuId = id.Value.ToString();
        }
        return cpuId;
    }

    private static string Win32GetMotherboardSerialNumber(){
        var management = new ManagementClass("Win32_BaseBoard");
        var collection = management.GetInstances();
        var boardId = string.Empty;
        foreach(var obj in collection){
            var id = obj.Properties["SerialNumber"];
            if(id is not null) boardId = id.Value.ToString();
        }
        return boardId;
    }
    #elif LINUX && MACOS
    
    #endif
}
