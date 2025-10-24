using System.Text.Json.Nodes;
using MiraiCL.Core.Network;

namespace MiraiCL.Core.Minecraft.Instance.Clients;

public interface IClient{
    Task<JsonNode?> GetJsonAsync();
    Task<DownloadItem[]?> AnalyzeLibrary();
    Task<DownloadItem[]?> GetMissingLibaray();
    string[] GetArgument();
}