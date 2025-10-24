using MiraiCL.Core.Network;
using System.Text.Json.Nodes;


namespace MiraiCL.Core.Minecraft.Projects.Modrinth;

public class ModrinthApiClient{
    public static async Task<JsonNode?> GetProject(string projectId){
        return await WebUtils.GetJsonRetryAsync("");
    }
}
