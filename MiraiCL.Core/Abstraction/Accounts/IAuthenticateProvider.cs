namespace MiraiCL.Core.Abstraction.Accounts.Providers;

public interface IAuthenticateProvider{
    Task AuthenticateAsync();
    Task RefreshAsync();
    Task ValidateAsync();
    Task InvalidateAsync();
}