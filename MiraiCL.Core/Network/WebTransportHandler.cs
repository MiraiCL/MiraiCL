using System.Net;
using System.Net.Http.Headers;
using MiraiCL.Core.Exts;

namespace MiraiCL.Core.Network;

public class WebTransportHandler:DelegatingHandler
{
    public bool AllowAutoRetry { get; set; } = true;

    public int MaxRedirect { get; set; } = 20;
    
    private int MaxRetry { get; set; } = 3;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var redirectCount = 0;
        for (var retryCount = 0; retryCount <= MaxRetry; retryCount++)
        {
            try
            {
                
                var response = await base.SendAsync(request, cancellationToken);
                if ((int)response.StatusCode is >= 300 and <= 399 &&
                    response.StatusCode != HttpStatusCode.NotModified)
                {
                    if (redirectCount > MaxRedirect) throw new WebException("Remote server redirect too many times.");
                    var req = request.Clone();
                    // https://developer.mozilla.org/zh-CN/docs/Web/HTTP/Reference/Status/302
                    if((int)response.StatusCode == 302) req.Method = HttpMethod.Get;
                    var redirect = response.Headers.Location?.ToString();
                    if(redirect.IsNullOrEmpty()) throw new HttpRequestException("Invalid redirect response,the location header must not null or empty.");
                    req.RequestUri = 
                        redirect.StartsWith("http") ? new Uri(redirect) : new Uri(request.RequestUri!, redirect);
                    retryCount--;
                    redirectCount++;
                    continue;
                }
                return response;
            }
            catch (HttpRequestException ex)
            {
                if (!AllowAutoRetry || retryCount == MaxRetry) throw new HttpRequestException("Make HttpRequest Failed.",ex);
            }
            
        }
        throw new HttpRequestException();
    }
}