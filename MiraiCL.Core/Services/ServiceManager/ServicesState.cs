namespace MiraiCL.Core.Services.ServiceManager;
public enum ServiceState{
    ///<summary>
    /// Indicates that the service should not be run automatically by the service manager. <br/>
    /// Services with this status can be started using ServiceManager.StartService. <br/>
    ///</summary>
    Manual,
    ///<summary>
    /// Runs during the application startup phase.<br/> 
    /// Services holding this state can use ServiceManager.Exit and ServiceManager.Restart to close or restart the program.
    ///</summary>
    Bootstrap,
    ///<summary>
    /// Run during the application loading phase, at which point you can use ServiceContext.(Level) to log messages.
    ///</summary>
    Loading,
    ///<summary>
    ///Executed when the application has completed the loading phase.<br/>
    ///For services that require other components, they should be loaded at this stage.
    ///</summary>
    Loaded,
    WindowLoading,
    WindowLoaded,
    Running,
    WindowClosed,
    Exited
}