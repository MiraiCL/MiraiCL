namespace MiraiCL.Core;

public static class UPath
{
    public static string DataPath = Environment.SpecialFolder.ApplicationData.ToString();

    public static readonly string LogPath = Path.Combine(DataPath, "Logs");
}