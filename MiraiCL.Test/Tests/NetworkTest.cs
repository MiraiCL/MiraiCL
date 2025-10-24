using MiraiCL.Core.Network;

namespace MiraiCL.Test.Tests;


public class NetworkTest{
    [Test]
    public void StringDownloadTest(){
        var str = WebUtils.GetStringRetryAsync("https://littleskin.cn").GetAwaiter().GetResult();
        Console.WriteLine(str);
    }
    [Test]
    public void RangeDownloadTest(){
        var bytes = WebUtils.GetContentWithSize("https://dldir1.qq.com/qqfile/qq/PCQQ9.7.17/QQ9.7.17.29225.exe",0,16384).GetAwaiter().GetResult();
        Console.WriteLine(bytes.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult().Length);
    }
    [Test]
    public void ByteArrayDownloadTest(){
        var bytes = WebUtils.GetContentWithSize("https://speed.cloudflare.com/__down?bytes=10485760").GetAwaiter().GetResult();
        Console.WriteLine(bytes.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult().Length);
    }
    [Test]
    public void TestPost(){
        var result = WebUtils.UploadContent("https://jsonplaceholder.typicode.com/posts",HttpMethod.Post,(new CancellationTokenSource()).Token);
        Console.WriteLine(result.Result.Content.ReadAsStringAsync().Result);
    }
}

