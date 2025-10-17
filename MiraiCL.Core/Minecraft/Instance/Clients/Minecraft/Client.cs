using System.Text.Json.Nodes;

namespace MiraiCL.Core.Minecraft.Instance.Clients.Minecraft;

public class MinecraftClient{
    private JsonNode VersionJson;
    public static MinecraftClient Parse(string version){
        return new MinecraftClient();
    }

}