namespace SharedComponents.Storage;

public sealed class TempStorageException
    : BaseCisException
{
    public TempStorageException(int code,  string message)
        : base(code, message)
    {
    }
}
