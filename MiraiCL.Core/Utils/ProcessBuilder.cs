using System.Diagnostics;

namespace MiraiCL.Core.Utils;

public class ProcessBuilder{
    private Process _process = new();
    private ProcessBuilder(string executePath){ _process.StartInfo.ArgumentList.Add(executePath); }

    public static ProcessBuilder Create(string path) => new ProcessBuilder(path);

    
}

