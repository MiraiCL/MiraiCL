// SetupUtils.cs
using System;
using System.Collections.Generic;
#if WINDOWS
using Microsoft.Win32;
#endif
using MiraiCL.Core.Abstraction.App.Setup;

namespace MiraiCL.Core.App.Setup;

public class SetupUtils : ISetupUtils
{
    public SetupUtils(SetupProviders providers)
    {
        _providers = providers;
#if WINDOWS
        if (_providers == SetupProviders.Registry)
        {
            // Ensure the key exists; CreateSubKey returns the opened key
            ConfigRegistry = Registry.CurrentUser.CreateSubKey("SOFTWARE/MiraiCLDev/MiraiCL", true);
        }
        else
        {
            ConfigRegistry = null;
        }
#endif
        ConfigFile = _providers == SetupProviders.Local ? new Dictionary<string, object?>() : null;
    }

    private readonly SetupProviders _providers;

#if WINDOWS
    public RegistryKey? ConfigRegistry { get; }
#endif
    public Dictionary<string, object?>? ConfigFile { get; }

    public object? Get(string key)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        switch (_providers)
        {
#if WINDOWS
            case SetupProviders.Registry:
                try
                {
                    return ConfigRegistry?.GetValue(key);
                }
                catch
                {
                    return null;
                }
#endif
            default:
                if (ConfigFile == null) return null;
                return ConfigFile.TryGetValue(key, out var v) ? v : null;
        }
    }

    public void Set(string key, object value)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        switch (_providers)
        {
#if WINDOWS
            case SetupProviders.Registry:
                if (ConfigRegistry == null) throw new InvalidOperationException("Registry is not available.");
                // Registry accepts certain types; assume caller provides valid type.
                ConfigRegistry.SetValue(key, value);
                break;
#endif
            default:
                if (ConfigFile == null) throw new InvalidOperationException("Local config is not available.");
                ConfigFile[key] = value;
                break;
        }
    }

    public void Delete(string key)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        switch (_providers)
        {
#if WINDOWS
            case SetupProviders.Registry:
                if (ConfigRegistry == null) return;
                try
                {
                    ConfigRegistry.DeleteValue(key, false);
                }
                catch
                {
                    // ignore if not exist
                }
                break;
#endif
            default:
                ConfigFile?.Remove(key);
                break;
        }
    }
}