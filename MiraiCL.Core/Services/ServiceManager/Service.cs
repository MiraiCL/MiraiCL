namespace MiraiCL.Core.Services.ServiceManager;

public abstract class Service{
    public required string ServiceName;
    public required string ServiceIdentifier;
    public bool SupportAsync {get;set;} = false;
    public abstract void Startup();
    public abstract void Shutdown();
}