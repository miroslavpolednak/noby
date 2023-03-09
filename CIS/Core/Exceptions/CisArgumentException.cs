namespace CIS.Core.Exceptions;

public sealed class CisArgumentException
    : BaseCisException
{
    public string? Argument { get; init; }

    public CisArgumentException(int exceptionCode, string message, string argument)
        : base(exceptionCode, message)
    {
        Argument = argument;
    }

    public CisArgumentException(int exceptionCode, string message)
        : base(exceptionCode, message)
    {
    }
}
