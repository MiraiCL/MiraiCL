namespace MiraiCL.Core.Abstraction.Runtime.Win32.Com.MediaFoundation;

public interface IMFBlock{
    public int GetClockCharacteristics(out double pdwCharacteristics);
    public int GetCorrelatedTime(out double dwReserved,out IntPtr pllClockTime,out IntPtr phnsSystemTime);
    public int GetProperties(out IntPtr pClockProperties);
    public int GetState(double dwReserved,out IntPtr peClockState);
}

