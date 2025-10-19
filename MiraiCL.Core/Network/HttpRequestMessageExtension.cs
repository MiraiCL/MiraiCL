namespace MiraiCL.Core.Network;

public static class HttpRequestMessageExtension
{
    public static HttpRequestMessage Clone(this HttpRequestMessage request)
    {
        var req = new HttpRequestMessage(request.Method, request.RequestUri);
        if (request.Content is not null) req.Content = new StreamContent(request.Content.ReadAsStream());
        foreach (var kvp in request.Headers)
        {
            req.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
        }

        if (request.Content?.Headers.Count() != 0)
        {
            foreach (var kvp in request.Content?.Headers!)
            {
                req.Content?.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);
                
            }
        }

        return req;
    }
}