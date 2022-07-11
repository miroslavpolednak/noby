namespace CIS.Core.Exceptions;

public sealed class CisServiceUnavailableException : BaseCisException
{
    public string ServiceName { get; init; }
    public string MethodName { get; init; }

    public CisServiceUnavailableException(string serviceName, string methodName, string message) 
        : base(5, message) 
    {
        MethodName = methodName;
        ServiceName = serviceName;
    }
}
