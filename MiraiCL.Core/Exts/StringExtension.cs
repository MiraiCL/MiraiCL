using System.Diagnostics.CodeAnalysis;
using System.Text;
namespace MiraiCL.Core.Exts;

public static class StringExtension{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? text) 
        => string.IsNullOrEmpty(text);

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? text) =>
        string.IsNullOrWhiteSpace(text);

    public static HttpRequestMessage CreateRequest(this string uri,HttpMethod method) => 
        new HttpRequestMessage(method,uri);

    public static Uri ToURI(this string text) => new Uri(text);

    public static byte[] GetBytes(this string text,Encoding? encode = null) => (encode ?? Encoding.UTF8).GetBytes(text);

}