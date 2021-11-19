namespace CIS.InternalServices.Storage.Abstraction.Exceptions;

internal sealed class InvalidSessionIdException : Core.Exceptions.BaseCisException
{
    public InvalidSessionIdException(int code, string message)
        : base(code, message)
    { }
}
