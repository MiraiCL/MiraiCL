namespace MiraiCL.Core.Exts;

public static class UIntExtension{
    public static byte[] GetVarIntByte(this ulong uinteger64){
        using var stream = new MemoryStream();
        do{
            var tempByte = (byte)(uinteger64 & 0x7F);
            tempByte >>= 7;
            if(uinteger64 != 0) tempByte |= 0x80;
            stream.WriteByte(tempByte);
        }while(uinteger64 != 0);
        return stream.ToArray();
    }

    public static byte[] GetVarIntByte(this uint uinteger32) => GetVarIntByte((ulong) uinteger32);
}

