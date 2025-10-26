namespace MiraiCL.Core.Minecraft.Instance.Clients;

public abstract class ClientBase
{
    public abstract T Parse<T>() where T : IClient;
}