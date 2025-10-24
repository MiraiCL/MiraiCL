using System;
using System.Collections.Generic;
using MiraiCL.Core.Abstraction.App.Setup;

namespace MiraiCL.Core.App.Setup
{
    public class SetupProvider
    {
        public SetupProvider(SetupProviders providers)
        {
            _utils = new SetupUtils(providers);
            _providers = providers;
        }

        private readonly SetupUtils _utils;
        private readonly SetupProviders _providers;
        private bool _loaded;
        private Dictionary<string, ISetupValue>? cacheConfig;
        public object SyncLock = new();

        // 加载指定的 key 列表，返回 ISetupValue 的字典（缓存）
        public Dictionary<string, ISetupValue> Load(IEnumerable<string> setups, bool isForceReload = false)
        {
            lock (SyncLock)
            {
                if (_loaded && !isForceReload && cacheConfig != null) return cacheConfig;

                var setup = new Dictionary<string, ISetupValue>(StringComparer.OrdinalIgnoreCase);
                foreach (var key in setups)
                {
                    var raw = _utils.Get(key);
                    var sv = new SetupValue<object>
                    {
                        Value = raw,
                        Name = key,
                        // 如果没有值，则视为 Undefined，否则 Setted
                        // 注意：如果需要区分 Locked/Encrypted 等状态，应在其它逻辑中处理
                    };
                    if (raw is null) sv.Reset();
                    else sv.SetValue(raw);

                    setup[key] = sv;
                }

                cacheConfig = setup;
                _loaded = true;
                return setup;
            }
        }

        // 将字典写回 provider（registry 或 本地字典）
        public void Save(Dictionary<string, ISetupValue> setup)
        {
            if (setup is null) throw new ArgumentNullException(nameof(setup));
            lock (SyncLock)
            {
                cacheConfig = new Dictionary<string, ISetupValue>(setup, StringComparer.OrdinalIgnoreCase);
                foreach (var kvp in setup)
                {
                    try
                    {
                        var value = kvp.Value?.GetValue();
                        if (value is null)
                        {
                            _utils.Delete(kvp.Key);
                        }
                        else
                        {
                            _utils.Set(kvp.Key, value);
                        }
                    }
                    catch
                    {
                        // 忽略单个键写入错误（可按需扩展为日志/抛出）
                    }
                }
            }
        }
    }
}