using MiraiCL.Core.Network;
using System.Text.Json.Nodes;
using MiraiCL.Core.Minecraft.Instance.Clients.Minecraft;
using MiraiCL.Core.Services.Logger;

public static class McVersion{
    public static JsonNode? VersionIndexJson;
    public static async Task UpdateVersionIndex(bool requireRefresh = false){
        if(VersionIndexJson is not null && requireRefresh) return;
        var logger = LogService.CurrentLogger;
        foreach(var source in DownloadSourcePolicy.GetPerferVersionSource()){
            logger?.Debug("Minecraft",$"开始下载版本列表：{source.SourceName}");
            VersionIndexJson = await WebUtils.GetJsonRetryAsync((source.VersionIndexV2??source.VersionIndex)!);
        }
    }
    public static void GetDescription(){

    }
    public static async Task<IEnumerable<JsonNode?>?> Lookup(string mcVersion){
        await UpdateVersionIndex();
        return VersionIndexJson?["versions"]?.AsArray().Where(version => version?["id"]?.ToJsonString().Contains(mcVersion) ?? false);
    }
    public static async Task<JsonNode?> GetVersionMeta(string mcVersion){
        await UpdateVersionIndex();
        return VersionIndexJson?["versions"]?.AsArray().Single(version => version?["id"]?.ToJsonString().ToLower() == mcVersion);
    }
}