using System.Net;
using MiraiCL.Core.Exts;
namespace MiraiCL.Core.Services.Network;

public class HttpProxy : IWebProxy
{
    public ICredentials? Credentials { get; set; }

    public bool DisableProxy {get;set;}

    private string? _proxyAddress;

    public string ProxyAddress {
        get => _proxyAddress ?? string.Empty; 
        set
        {
            if(!(value.StartsWith("http") || value.StartsWith("socks5"))) value = $"http://{value}";
            var parsedUri = value.ToURI();
            _scheme = parsedUri.Scheme;
            _proxyAddress = parsedUri.Host;
        }
    }

    public int ProxyPort {get;set;}

    private IWebProxy? _currentProxy;

    public bool NeedRefresh {get;set;}

    private string? _scheme;

    public IWebProxy CurrentProxy{
        get {
            if (!NeedRefresh && _currentProxy is not null) return _currentProxy;
            if(!ProxyAddress.IsNullOrEmpty()){
                _currentProxy = new WebProxy(ProxyAddress,true);
            }
            else _currentProxy = SystemProxy;
            if(Credentials is not null) _currentProxy.Credentials = Credentials;
            return _currentProxy;
        }
    }

    private IWebProxy SystemProxy
    {
        get => HttpClient.DefaultProxy;
    }

    public Uri GetProxy(Uri destination) => CurrentProxy.GetProxy(destination);

    public bool IsBypassed(Uri host) => DisableProxy || CurrentProxy.IsBypassed(host);
}