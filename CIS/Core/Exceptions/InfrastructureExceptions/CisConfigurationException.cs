namespace CIS.Core.Exceptions;

public sealed class CisConfigurationException 
    : BaseCisException
{
    public CisConfigurationException(int exceptionCode, string? message) : base(exceptionCode, message)
    { }
}
