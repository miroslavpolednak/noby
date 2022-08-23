namespace CIS.Core.Exceptions;

public class CisConflictException : BaseCisException
{
    public CisConflictException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    {
    }

    public CisConflictException(int exceptionCode, string message)
        : base(exceptionCode, message)
    {
    }
}
