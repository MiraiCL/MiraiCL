using System.Text.Json;

namespace MiraiCL.Core;

public static class JsonConfig{
    public static readonly JsonSerializerOptions options = new(){
        WriteIndented = true
    };
}