using MiraiCL.Core.LinkLobby.Scaffolding;

namespace MiraiCL.Test.Tests;

public class LobbyCodeTest{
    [Test]
    public void TestCode() => Console.WriteLine((new LobbyCode()).GenerateCode());
}