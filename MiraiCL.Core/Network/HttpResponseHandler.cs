using System.Net;
using System.Net.Http.Headers;
using SharpCompress.Readers;

namespace MiraiCL.Core.Network;

public class HttpResponseHandler(HttpResponseMessage response):IDisposable{
    private bool _alreadyRead;
    public Stream ContentStream 
    {
        get
        {
            if(_alreadyRead) throw new InvalidOperationException();
            _alreadyRead = true; 
            return response.Content.Headers.ContentEncoding.Any(encoding => encoding.ToLower() == "zstd")
                    ? ReaderFactory.Open(response.Content.ReadAsStream()).OpenEntryStream():response.Content.ReadAsStream();
        }
    }
    public void Check() => response.EnsureSuccessStatusCode();

    public void Dispose() => response.Dispose();

    public HttpResponseHeaders Headers => response.Headers;

    public bool IsSuccess => response.IsSuccessStatusCode;

    public HttpStatusCode Status => response.StatusCode;
}