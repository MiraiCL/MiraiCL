namespace MiraiCL.Core.LinkLobby.Scaffolding;

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// A lobby code that complies with the Scaffolding protocol specification, using modular checksum (mod 7).
/// </summary>
public class LobbyCode
{
    private const int BaseRadix = 34;
    private const int CheckMod = 7;
    private const int GroupCount = 4;
    private const int GroupSize = 4;
    private const int TotalChars = GroupCount * GroupSize; // 16

    private static readonly char[] ValidChars =
        "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();

    private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

    private static readonly Regex CodePattern =
        new Regex(@"^U\/([0-9A-Z]{4}-){3}[0-9A-Z]{4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string NetworkName { get; private set; } = string.Empty;
    public string Secret { get; private set; } = string.Empty;

    public static int GetSecureInt() => RandomNumberGenerator.GetInt32(ValidChars.Length);

    public static char GetRandomChar() => ValidChars[RandomNumberGenerator.GetInt32(ValidChars.Length)];

    public static LobbyCode Parse(string code)
    {
        if (!Validate(code)) throw new ArgumentException("Invalid lobby code", nameof(code));

        var body = code.Substring(2);
        var parts = body.Split('-', StringSplitOptions.RemoveEmptyEntries);
        return new LobbyCode
        {
            NetworkName = $"scaffolding-mc-{parts[0]}-{parts[1]}",
            Secret = $"{parts[2]}-{parts[3]}"
        };
    }

    public string GenerateCode()
    {
        var chars = new char[TotalChars];
        for (var i = 0; i < TotalChars - 1; i++)
            chars[i] = GetRandomChar();

        chars[TotalChars - 1] = CalculateLastChar(chars.Take(TotalChars - 1).ToArray());

        var sb = new StringBuilder("U/");
        for (var i = 0; i < TotalChars; i++)
            sb.Append(i > 0 && i % GroupSize == 0 ? '-' : "").Append(chars[i]);

        var code = sb.ToString();
        NetworkName = $"scaffolding-mc-{code.Substring(2).Split('-')[0]}-{code.Substring(2).Split('-')[1]}";
        Secret = $"{code.Substring(2).Split('-')[2]}-{code.Substring(2).Split('-')[3]}";
        return code;
    }

    public static bool Validate(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || !CodePattern.IsMatch(code)) return false;
        
        var body = code.ToUpperInvariant().Replace("U/", "").Replace("-", "");
        return body.Length == TotalChars && ChecksumIsValid(body.ToCharArray());
    }

    private static bool ChecksumIsValid(char[] chars)
    {
        try
        {
            var valueMod = 0L;
            for (var i = 0; i < chars.Length; i++)
            {
                var v = CharToValue(chars[i]);
                var pow = PowMod(BaseRadix % CheckMod, i, CheckMod);
                valueMod = (valueMod + v * pow) % CheckMod;
            }
            return valueMod % CheckMod == 0;
        }
        catch { return false; }
    }

    private static char CalculateLastChar(char[] first15Chars)
    {
        var currentMod = 0L;
        for (var i = 0; i < first15Chars.Length; i++)
        {
            var v = CharToValue(first15Chars[i]);
            var pow = PowMod(BaseRadix % CheckMod, i, CheckMod);
            currentMod = (currentMod + v * pow) % CheckMod;
        }

        var basePow15 = PowMod(BaseRadix % CheckMod, TotalChars - 1, CheckMod);
        var inverse = ModInverse(basePow15, CheckMod);
        var needed = ((CheckMod - currentMod) % CheckMod) * inverse % CheckMod;

        for (var i = 0; i < ValidChars.Length; i++)
            if (i % CheckMod == needed) return ValidChars[i];
        throw new ArithmeticException("Checksum calculation failed");
    }

    private static int CharToValue(char c)
    {
        for (var i = 0; i < ValidChars.Length; i++)
        {
            if (string.Equals(ValidChars[i].ToString(), c.ToString(), StringComparison.OrdinalIgnoreCase))
                return i;
        }
        
        throw new ArgumentException($"Invalid character '{c}' for Base34 conversion", nameof(c));
    }

    private static long PowMod(long baseValue, int exponent, int modulus)
    {
        if (modulus == 1) return 0;
        long result = 1;
        baseValue %= modulus;
        while (exponent > 0)
        {
            if ((exponent & 1) == 1) result = (result * baseValue) % modulus;
            baseValue = (baseValue * baseValue) % modulus;
            exponent >>= 1;
        }
        return result;
    }

    private static long ModInverse(long a, long m)
    {
        var m0 = m;
        long x0 = 0, x1 = 1;
        long aa = a % m;
        while (aa > 1)
        {
            var q = aa / m;
            var t = m;
            m = aa % m;
            aa = t;
            var temp = x0;
            x0 = x1 - q * x0;
            x1 = temp;
        }
        if (x1 < 0) x1 += m0;
        if (aa != 1) throw new ArithmeticException("Modular inverse does not exist");
        return x1;
    }
}