using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using MiraiCL.Core.Abstraction.Runtime.Win32.Com.MediaFoundation;

[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("90377834-21D0-4dee-8214-BA2E3E6C1127")]
public interface IMFMediaSession{
    public int ClearTopologies();
    public int Close();
    public int GetClock(out IMFBlock ppClock);

}