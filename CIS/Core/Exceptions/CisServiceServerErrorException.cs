namespace CIS.Core.Exceptions;

public sealed class CisServiceServerErrorException : BaseCisException
{
    public string ServiceName { get; init; }
    public string MethodName { get; init; }

    public CisServiceServerErrorException(string serviceName, string methodName, string message)
        : base(15, message)
    {
        MethodName = methodName;
        ServiceName = serviceName;
    }
}
