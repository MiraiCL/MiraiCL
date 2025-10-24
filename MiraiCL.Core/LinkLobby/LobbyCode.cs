using System.Security.Cryptography;
using System.Text;


namespace MiraiCL.Core.LinkLobby;
///<summary>
/// An lobby code that complies with the Scaffolding protocol specification, based on mathematical calculations.
///</summary>
public class LobbyCode{
    private static readonly char[] ValidChars = 
        "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();
    private static readonly RandomNumberGenerator SecureGenerator = RandomNumberGenerator.Create();
    
    public static int GetSecureInt(){
        return RandomNumberGenerator.GetInt32(0,34);
    }

    public string NetworkName;

    public string Secret;

    public static LobbyCode Parse(string code){
        if(!Validate(code)) throw new ArgumentException("Invalid lobby code");        
        var lobbyCode = new LobbyCode();
        var codeSplited = code.Replace("U/","").Split("-");
        lobbyCode.NetworkName = $"scaffolding-mc-{codeSplited[0]}-{codeSplited[1]}";
        lobbyCode.Secret = $"{codeSplited[2]}-{codeSplited[3]}";
        return lobbyCode;
    }
    
    public string GenerateCode(){
        var codeBuilder = new StringBuilder("U/");
        
        for (var i = 0; i < 16; i++)
        {
            if (i == 4 || i == 8 || i == 12)
                codeBuilder.Append('-');
                
            codeBuilder.Append(GetRandomChar());
        }
        var code = codeBuilder.ToString();
        var codeSplited = code.Replace("U/","").Split("-");
        NetworkName = $"scaffolding-mc-{codeSplited[0]}-{codeSplited[1]}";
        Secret = $"{codeSplited[2]}-{codeSplited[3]}";
        return code;
    }
    
    public static char GetRandomChar(){
        var randomByte = new byte[1];
        SecureGenerator.GetBytes(randomByte);
        return ValidChars[randomByte[0] % ValidChars.Length];
    }

    private static long PowMod(long baseValue, long exponent, long modulus)
    {
        var result = 1L;
        baseValue %= modulus;
        
        while (exponent > 0)
        {
            if ((exponent & 1) == 1)
                result = (result * baseValue) % modulus;
            
            baseValue = (baseValue * baseValue) % modulus;
            exponent >>= 1;
        }
        
        return result;
    }
    
    ///<summary>
    /// Compute modular inverse: a⁻¹ mod m <br/>
    /// Using the extended Euclidean algorithm
    ///</summary>
    private static long ModInverse(long a, long m)
    {    
        var m0 = m;
        var y = 0L;
        var x = 1L;
        
        if (m == 1)
            return 0;
        
        while (a > 1)
        {
            var q = a / m;
            var t = m;
            
            m = a % m;
            a = t;
            t = y;
            
            y = x - q * y;
            x = t;
        }
        
        if (x < 0)
            x += m0;
        
        return x;
    }

    private static long CalculateBasePowerMod7(int exponent)
    {
        
        var baseValue = 34L % 7L; // = 6
        return PowMod(baseValue, exponent, 7);
    }

    private static int CharToValue(char c)
    {
        if (char.IsDigit(c)) return c - '0';
        return c switch
        {
            >= 'A' and <= 'H' => 10 + (c - 'A'),
            >= 'J' and <= 'N' => 18 + (c - 'J'),
            >= 'P' and <= 'Z' => 23 + (c - 'P'),
            _ => throw new ArgumentException($"Invalid char: {c}")
        };
    }

    private static char CalculateLastChar(char[] first15Chars)
    {
        var currentValueMod7 = CalculateValueMod7(first15Chars);
        
        // 计算 34^15 mod 7
        var basePowerMod7 = CalculateBasePowerMod7(15);
        
        // 数学方程: (currentValueMod7 + lastCharValue * basePowerMod7) ≡ 0 (mod 7)
        // 得出: lastCharValue ≡ (-currentValueMod7) * basePowerMod7⁻¹ (mod 7)
        
        var remainder = currentValueMod7 % 7;
        var neededRemainder = (7 - remainder) % 7; // -currentValueMod7 mod 7
        
        // 计算逆元
        var inverseBaseMod7 = ModInverse(basePowerMod7, 7);
        
        // 计算最后一个字符的 Mod 7 值
        var lastCharValueMod7 = (neededRemainder * inverseBaseMod7) % 7;
        
        // 索引 % 7 == lastCharValueMod7
        for (var i = 0; i < ValidChars.Length; i++)
        {
            if (i % 7 == lastCharValueMod7)
                return ValidChars[i];
        }
        
        throw new ArithmeticException();
    }

    private static long CalculateValueMod7(char[] chars)
    {
        
        var valueMod7 = 0L;
        
        for (var i = 0; i < chars.Length; i++)
        {
            var charValue = CharToValue(chars[i]);
            var basePowerMod7 = CalculateBasePowerMod7(i);
            valueMod7 = (valueMod7 + charValue * basePowerMod7) % 7;
        }
        
        return valueMod7;
    }

    private static bool _check(string code){
        try{
            code = code.Replace("-","").Replace("U/","");
            ArgumentException.ThrowIfNullOrEmpty(code);
            var chars = code.ToCharArray();
            if(chars.Length != 16) return false;
            var value = 0L;
            for (var i = 0; i < chars.Length; i++)
            {
                int charValue = CharToValue(chars[i]);
                // 小端序：位置i的字符贡献 34^i 的权重
                value += charValue * (long)Math.Pow(34, i);
            }
            return value %7 == 0;
        }catch(ArgumentException) {
            // invalid char    
        }
        return false;
    }

    public static bool Validate(string code) => code.StartsWith("U/",StringComparison.OrdinalIgnoreCase) && _check(code);
    
}