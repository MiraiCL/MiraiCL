namespace MiraiCL.Core.Abstraction.Network;

///<summary>
/// Socket 传输层接口
///</summary>
public interface ISocketTransport{
    public Task<bool> ConnectAsync();
    public bool Connect();
    public bool Bind();
    public void SetForwardRule();
    public Task SendAsync(byte[] buffer,CancellationToken? token);
}