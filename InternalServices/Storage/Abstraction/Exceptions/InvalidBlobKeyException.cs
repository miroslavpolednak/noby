namespace CIS.InternalServices.Storage.Abstraction.Exceptions;

internal sealed class InvalidBlobKeyException : Core.Exceptions.BaseCisException
{
    public InvalidBlobKeyException(int code, string message)
        : base(code, message)
    { }
}
