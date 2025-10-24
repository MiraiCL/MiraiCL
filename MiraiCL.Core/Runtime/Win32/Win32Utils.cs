#if WINDOWS

namespace MiraiCL.Core.Runtime.Win32;

public static class Win32Utils{
    [DllImport("user32.dll")]
    private static extern void OpenClipboard();

    [DllImport("user32.dll")]
    private static extern void CloseClipboard();

    [DllImport("user32.dll")]
    private static extern void EmptyClipboard();

    [DllImport("user32.dll")]
    private static extern void GetClipboardData(uint format);

    [DllImport("user32.dll")]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalFree(IntPtr hMem);

}

#endif