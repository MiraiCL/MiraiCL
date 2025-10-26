using MiraiCL.Core.App.Setup;

namespace MiraiCL.Core.Minecraft.Instance.Clients.Minecraft;

public static class DownloadSourcePolicy{
    private static readonly Source Mojang = new(){
        VersionIndexV2 = "https://piston-meta.mojang.com/mc/game/version_manifest_v2.json",
        VersionIndex = "https://piston-meta.mojang.com/mc/game/version_manifest.json",
        MavenServer = "https://libraries.minecraft.net",
        JavaIndexUrl = "https://piston-meta.mojang.com/v1/products/java-runtime/2ec0cc96c44e5a76b9c8b7c39df7210883d12871/all.json",
        AssetsServer = "https://resources.download.minecraft.net",
        SourceName = "Mojang"
    };
    ///<summary>
    /// 获取版本列表下载地址（现阶段暂时只返回 Mojang 官方源的地址）
    ///</summary>
    public static Source[] GetPerferVersionSource(){
        return [Mojang];
    }
}

public record Source{
    public string? VersionIndexV2;
    public string? VersionIndex;
    public string? MavenServer;
    public string? JavaIndexUrl;
    public string? AssetsServer;
    public required string SourceName;
}