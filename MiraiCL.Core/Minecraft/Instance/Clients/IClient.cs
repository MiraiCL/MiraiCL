using System.Text.Json.Nodes;

namespace MiraiCL.Core.Minecraft.Instance.Clients;

public interface IClient{
    Task<JsonNode> GetJsonAsync();
    Task AnalyzeLibrary();
    
}