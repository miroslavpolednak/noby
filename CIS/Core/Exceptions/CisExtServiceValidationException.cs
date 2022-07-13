namespace CIS.Core.Exceptions;

public class CisExtServiceValidationException : CisValidationException
{
    public CisExtServiceValidationException(string message)
        : base(BaseCisException.UnknownExceptionCode, message)
    {
    }

    public CisExtServiceValidationException(IEnumerable<(string Key, string Message)> errors, string message = "")
        : base(BaseCisException.UnknownExceptionCode, message)
    {
        Errors = errors.ToList().AsReadOnly();
    }
}
