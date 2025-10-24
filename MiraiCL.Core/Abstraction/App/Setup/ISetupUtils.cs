namespace MiraiCL.Core.Abstraction.App.Setup;

public interface ISetupUtils
{
    void Set(string key, object value);
    object? Get(string key);
    void Delete(string key);
}