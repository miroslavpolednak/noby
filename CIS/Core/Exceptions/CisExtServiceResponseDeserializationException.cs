namespace CIS.Core.Exceptions;

public sealed class CisExtServiceResponseDeserializationException 
    : BaseCisException
{
    public CisExtServiceResponseDeserializationException(int exceptionCode, string serviceName, string url, string responseModelType)
        : base(exceptionCode, $"{serviceName} response from {url} can not be deserialized to {responseModelType}")
    { }
}
