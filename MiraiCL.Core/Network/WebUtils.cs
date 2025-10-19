


using System.Net;
using System.Net.Http.Headers;

namespace MiraiCL.Core.Network;

public static class WebUtils
{
    private static readonly WebTransportHandler Handler = new()
    {
        InnerHandler = new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15),
            EnableMultipleHttp2Connections = true,
            AutomaticDecompression = DecompressionMethods.All,
            EnableMultipleHttp3Connections = true,
            AllowAutoRedirect = false,
            UseProxy = true,
            Proxy = HttpWebProxy.Instance
        }
    };

    private static readonly HttpClient Client = new HttpClient(Handler);
    public static async Task<string> GetStringRetryAsync(string uri,CancellationToken token) =>
        await Client.GetStringAsync(uri,token);

    public static async Task<HttpResponseMessage> UploadContent(
        string host,
        HttpMethod method,
        CancellationToken token,
        HttpContent? content = null,
        Dictionary<string,string>? Headers = null)
    {
        using var request = new HttpRequestMessage(method, host);
        request.Content = content;
        _ = Headers?.Any(kvp =>
        {
            if (!request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value))
                request.Content?.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
            return true;
        });
        return await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
    }

    public static async Task<HttpResponseMessage> GetResourceDetailsAsync(string host,CancellationToken token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Head, host);
        return await Client.SendAsync(request,HttpCompletionOption.ResponseHeadersRead,token);
    }

    public static async Task<HttpResponseMessage> GetContentWithSize(string host,
        long staredSize = 0, long endedSize = 0,CancellationToken? token = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, host);
        if (endedSize != 0)
        {
            request.Headers.Range = new RangeHeaderValue(staredSize, endedSize);
        }

        return await Client.SendAsync(request,HttpCompletionOption.ResponseHeadersRead,token??CancellationToken.None);
    }

    
}


public static class HttpRequestSettings
{
    public static HttpRequestOptionsKey<bool> DisallowRetry = new();
}
