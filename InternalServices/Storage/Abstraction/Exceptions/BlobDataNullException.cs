namespace CIS.InternalServices.Storage.Abstraction.Exceptions;

internal sealed class BlobDataNullException : Core.Exceptions.BaseCisException
{
    public BlobDataNullException(int code, string message)
        : base(code, message)
    { }
}
