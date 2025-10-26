using System.Text.Json.Nodes;
using MiraiCL.Core.Exceptions;
using MiraiCL.Core.Network;

namespace MiraiCL.Core.Minecraft.Instance.Clients.Minecraft;

public class MinecraftClient/*:IClient*/{
    private JsonNode? _versionJson;
    private JsonNode? _versionMeta;
    public Version? MinecraftVersion;
    public static async Task<MinecraftClient> Parse(string version){
        return new MinecraftClient(){
            _versionJson = await McVersion.GetVersionMeta(version)
        };
    }
    /*
    public static IClient[] ParseFromLocal(string jsonPath){
        return [new MinecraftClient()]
    }
    */

    public async Task<JsonNode?> GetJsonAsync(){
        ArgumentNullException.ThrowIfNull(MinecraftVersion);
        _versionMeta ??= await McVersion.GetVersionMeta(MinecraftVersion.ToString());
        _versionJson ??= await WebUtils
                .GetJsonRetryAsync((_versionMeta ?? throw new VersionNotFoundException($"Require version {MinecraftVersion} is invalid or not found."))["url"]!.ToString());
        return _versionJson;    
    }
/*
    public async Task<DownloadItem[]> AnalyzeLibrary(){
        var patches = _versionJson?["patches"];
        if(patches is not null){
            var weightMap = new Dictionary<int,JsonNode>();
            for(var patch in patches)
        }
    }
*/
    //public async Task<DownloadItem>

    public static int WriteLauncherProfile(string mcFolderPath){
        try{
            if(!Directory.Exists(mcFolderPath)) Directory.CreateDirectory(mcFolderPath);
            var profileJson = new JsonObject{
                ["profiles"] = new JsonObject{
                    ["LuoTianyi_YueZhengling"] = new JsonObject{
                        ["name"] = "LuoTianyi_YueZhengling"
                    }
                },
                ["clientToken"] = "2012017266cfff20150412ee00007412"
            };
            File.WriteAllText(Path.Combine(mcFolderPath,"launcher_profiles.json"),profileJson.ToJsonString(JsonConfig.options));
        }catch(UnauthorizedAccessException){
            return 1;
        }
        return 0;
    }


}