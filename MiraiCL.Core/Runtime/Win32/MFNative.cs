using System.Runtime.InteropServices;

namespace MiraiCL.Core.Runtime.Win32;

// MediaFoundation 初始化/反初始化（DLL 导入）
internal static class MFNative
{
    [DllImport("mfplat.dll")]
    public static extern int MFStartup(uint version, uint flags);

    [DllImport("mfplat.dll")]
    public static extern int MFShutdown();
}
