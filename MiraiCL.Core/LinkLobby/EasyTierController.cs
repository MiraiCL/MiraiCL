// Base on EasyTier FFI

using System.Runtime.InteropServices;

public class EasyTierController{
    private const string DllName = "EasyTier.dll";


    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int parse_config([MarshalAs(UnmanagedType.LPStr)] string cfgStr);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int run_network_instance([MarshalAs(UnmanagedType.LPStr)] string cfgStr);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int retain_network_instance(IntPtr instNames, int length);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int collect_network_infos(IntPtr infos, int maxLength);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void get_error_msg(out IntPtr errorMsg);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    private static extern void free_string(IntPtr str);

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyValuePair
    {
        public IntPtr Key;
        public IntPtr Value;
    }

    // 解析配置
    public static void ParseConfig(string config)
    {

        ArgumentException.ThrowIfNullOrEmpty(config);

        var result = parse_config(config);
        if (result < 0)
        {
            throw new COMException(GetErrorMessage());
        }
    }

    public static void RunNetworkInstance(string config)
    {
        ArgumentException.ThrowIfNullOrEmpty(config);

        int result = run_network_instance(config);
        if (result < 0)
        {
            throw new Exception(GetErrorMessage());
        }
    }

    public static void RetainNetworkInstances(string[] instanceNames)
    {
        IntPtr[]? namePointers = null;
        var namesPtr = IntPtr.Zero;

        try
        {
            if (instanceNames.Length > 0)
            {
                namePointers = new IntPtr[instanceNames.Length];
                for (int i = 0; i < instanceNames.Length; i++)
                {
                    ArgumentException.ThrowIfNullOrEmpty(instanceNames[i]);
                    namePointers[i] = Marshal.StringToHGlobalAnsi(instanceNames[i]);
                }

                namesPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>() * namePointers.Length);
                Marshal.Copy(namePointers, 0, namesPtr, namePointers.Length);
            }

            var result = retain_network_instance(namesPtr, instanceNames.Length);
            if (result < 0)
            {
                throw new Exception(GetErrorMessage());
            }
        }
        finally
        {
            if (namePointers != null)
            {
                foreach (var ptr in namePointers)
                {
                    if (ptr != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(ptr);
                    }
                }
            }

            if (namesPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(namesPtr);
            }
        }
    }

    public static KeyValuePair<string?, string?>[] CollectNetworkInfos(int maxLength)
    {
        IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf<KeyValuePair>() * maxLength);
        try
        {
            var count = collect_network_infos(buffer, maxLength);
            if (count < 0)
            {
                throw new COMException(GetErrorMessage());
            }

            var result = new KeyValuePair<string?, string?>[count];
            for (int i = 0; i < count; i++)
            {
                var kv = Marshal.PtrToStructure<KeyValuePair>(buffer + i * Marshal.SizeOf<KeyValuePair>());
                var key = Marshal.PtrToStringAnsi(kv.Key);
                var value = Marshal.PtrToStringAnsi(kv.Value);

                // 释放由 FFI 分配的字符串内存
                free_string(kv.Key);
                free_string(kv.Value);

                result[i] = new KeyValuePair<string?, string?>(key, value);
            }

            return result;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    // 获取错误信息
    private static string GetErrorMessage()
    {
        get_error_msg(out IntPtr errorMsgPtr);
        if (errorMsgPtr == IntPtr.Zero)
        {
            return "Unknown error";
        }

        var errorMsg = Marshal.PtrToStringAnsi(errorMsgPtr);
        free_string(errorMsgPtr); // 释放错误信息字符串
        return errorMsg ?? "Unknown Error";
    }

}