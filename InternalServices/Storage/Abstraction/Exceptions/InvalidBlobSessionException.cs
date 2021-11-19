namespace CIS.InternalServices.Storage.Abstraction.Exceptions;

internal sealed class InvalidBlobSessionException : Core.Exceptions.BaseCisException
{
    public InvalidBlobSessionException(int code, string message)
        : base(code, message)
    { }
}
