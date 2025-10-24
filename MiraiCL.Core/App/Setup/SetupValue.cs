// SetupValue.cs
using System;
using MiraiCL.Core.Abstraction.App.Setup;

namespace MiraiCL.Core.App.Setup;

public class SetupValue<T> : ISetupValue
{
    public string Name { get; set; } = "Unknown";
    public T? Value { get; set; }
    public SetupOption State { get; set; } = SetupOption.Undefined;

    public void Encrypt()
    {
        State = SetupOption.Encrypted;
        // 加密逻辑可在此处实现（目前只是标记）
    }

    public object? Decrypt()
    {
        // 如果有加密，应解密后返回真实值；当前只是直接返回值。
        return Value;
    }

    public void Reset() => State = SetupOption.Undefined;

    public void SetValue(T? value)
    {
        Value = value;
        State = value is null ? SetupOption.Undefined : SetupOption.Setted;
    }

    // 实现 ISetupValue 接口
    public void SetValue(object? value)
    {
        if (value is null)
        {
            Value = default;
            State = SetupOption.Undefined;
            return;
        }

        if (value is T t)
        {
            Value = t;
            State = SetupOption.Setted;
            return;
        }

        // 尝试进行通用转换（例如从 string 转到数值/布尔等）
        try
        {
            var converted = Convert.ChangeType(value, typeof(T));
            Value = (T?)converted;
            State = SetupOption.Setted;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Expected value of type {typeof(T).Name} for '{Name}', but got {value.GetType().Name}. Conversion failed: {ex.Message}", ex);
        }
    }

    public object? GetValue() => Value;

    public override string ToString()
    {
        return $"SetupValue Of {Name} (State={State})";
    }
}