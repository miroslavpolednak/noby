namespace CIS.InternalServices.Storage.Abstraction.Exceptions;

internal sealed class BlobNotFoundException : Core.Exceptions.BaseCisException
{
    public BlobNotFoundException(int code, string message)
        : base(code, message)
    { }
}
