// Setup.cs
using MiraiCL.Core.Abstraction.App.Setup;

namespace MiraiCL.Core.App.Setup;

public static class Setup
{
    // 默认定义表：仅做默认/元信息用途，实际值来自 provider
    private static readonly Dictionary<string, ISetupValue?> _setup = new()
    {
        ["Proxy"] = new SetupValue<string>(),
        ["IsChinaMainland"] = new SetupValue<bool>(),
        ["WindowHeight"] = new SetupValue<double>(),
        ["Theme"] = null,
        ["DownloadSourcePolicy"] = null,
        ["DownloadThread"] = null,
        ["VersionSourcePolicy"] = null,
        ["profiles"] = null,
        ["JvmArgument"] = null,
        ["DisableJavaLaunchWrapper"] = null,
        ["FoundtionHideen"] = null,
        ["MinecraftFolder"] = null,
        ["CurrentSelectedFolder"] = null,
        ["CurrentProfile"] = null,
        ["ApiServer"] = new SetupValue<string> { Value = "https://miraicl.github.io/packages/v1/versions.json", Name = "ApiServer", State = SetupOption.Locked },
        ["GitHubToken"] = null
    };

    public static SetupProviders Providers { get; } = ResolveProvider();

    private static SetupProvider _provider = new(Providers);

    private static SetupProviders ResolveProvider()
    {
        try
        {
            var raw = Secret.Read("CONFIG_PROVIDER");
            if (!string.IsNullOrEmpty(raw) && Enum.TryParse<SetupProviders>(raw, true, out var p))
                return p;
        }
        catch
        {
            // ignore
        }

        return SetupProviders.Local;
    }

    // 获取当前值：优先使用 provider 加载的缓存，其次返回默认定义中的值
    public static object? GetValue(string key)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        var loaded = _provider.Load(_setup.Keys, isForceReload: false);
        if (loaded != null && loaded.TryGetValue(key, out var v) && v != null)
        {
            return v.GetValue();
        }

        if (_setup.TryGetValue(key, out var def) && def != null)
        {
            return def.GetValue();
        }

        return null;
    }

    // 可选添加：通过 provider 保存并更新缓存
    public static void SetValue(string key, object? value)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        var loaded = _provider.Load(_setup.Keys, isForceReload: false);
        if (!loaded.ContainsKey(key))
        {
            loaded[key] = new SetupValue<object> { Name = key };
        }

        loaded[key].SetValue(value);
        _provider.Save(loaded);
    }

    // 强制刷新 provider 缓存
    public static void Reload() => _provider.Load(_setup.Keys, isForceReload: true);
}

public enum SetupProviders
{
    Registry,
    Local
}