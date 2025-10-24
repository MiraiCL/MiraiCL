namespace MiraiCL.Core.App;

public static class Secret{
    private readonly static Dictionary<string,string> _secrets = new(){
        ["CURSE_FORGE_API_KEY"] = "${CURSE}"
    };
    public static string Read(string secretName,bool readEnvOnDebug = true) 
        => readEnvOnDebug?Environment.GetEnvironmentVariable($"MiraiCL_{secretName}") ?? string.Empty:_secrets.GetValueOrDefault(secretName,string.Empty);
}