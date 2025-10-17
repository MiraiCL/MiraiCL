
using System.Net;
using System.Net.Http.Headers;

namespace MiraiCL.Core.Services.Network;

public class HttpRequestBuilder:IDisposable{
//    private static IHttpClientFactory? _factory;
    public HttpRequestMessage Request;
    private CancellationTokenSource? _cts;
    private HttpRequestBuilder(HttpRequestMessage request){
        Request = request;
    }

    public static HttpRequestBuilder Create(string uri,HttpMethod method){
        return new HttpRequestBuilder(new HttpRequestMessage(method,uri));
    }

    public void WithTimeout(int timeout){
        _cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));
    }

    public HttpRequestBuilder WithAuthorization(string tokenType,string token){
        Request.Headers.Add("Authorization",$"{tokenType} {token}");
        return this;
    }
/*
    public static HttpClient GetHttpClient(){
        if(_factory is null) throw new InvalidOperationException("HttpClientFactory Uninitiated.");
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.UserAgent.Clear();
        client.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("MiraiCL/9.0"));
        client.DefaultRequestVersion = HttpVersion.Version30;
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    }
    public static void Startup(IHttpClientFactory factory){
        _factory = factory;
    }
*/
    public void Dispose()
    {
        _cts?.Dispose();
        Request.Dispose();
    }
}