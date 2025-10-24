using MiraiCL.Core.LinkLobby;

namespace MiraiCL.Test.Tests;

public class LobbyCodeTest{
    [Test]
    public void TestCode() => Console.WriteLine((new LobbyCode()).GenerateCode());
}