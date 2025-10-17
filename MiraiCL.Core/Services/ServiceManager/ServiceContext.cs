using System;
namespace  MiraiCL.Core.Services.ServiceManager;

public class ServiceContext{
    
    public void Exit() => Environment.Exit(0);

    public void Restart() => Environment.Exit(0);

}

