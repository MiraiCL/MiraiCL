namespace MiraiCL.Core.Exceptions;

public class VersionNotFoundException:Exception{
    public VersionNotFoundException(){}

    public VersionNotFoundException(string message):base(message){}

    public VersionNotFoundException(string message,Exception innerException):base(message,innerException){}
}