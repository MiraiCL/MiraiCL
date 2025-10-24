namespace MiraiCL.Core.Abstraction.App.Setup;

public interface ISetupValue
{
    void Encrypt();
    object? Decrypt();
    object? GetValue();
    void SetValue(object? value);
}