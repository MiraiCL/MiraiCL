using System.Security;

namespace MiraiCL.Core.Accounts.Providers;

public class MicrosoftProvider:IAuthenticateProvider{

    public SecureString? AccessToken;

    private SecureString? _refreshToken;

    public async Task AuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    public async Task RefreshAsync()
    {
        throw new NotImplementedException();
    }

    public async Task ValidateAsync()
    {
        throw new NotImplementedException();
    }

    public async Task InvalidateAsync()
    {
        AccessToken = null;
        _refreshToken = null;
    }
}