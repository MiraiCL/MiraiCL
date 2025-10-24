namespace MiraiCL.Core.App;

public class Information{
    public static readonly ReleaseChannel Channel = ReleaseChannel.Develop;
}


public enum ReleaseChannel{
    Release,
    Preview,
    Develop,
}