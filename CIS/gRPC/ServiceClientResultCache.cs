namespace CIS.Infrastructure.gRPC;

public sealed class ServiceClientResultCache<TResult>
    where TResult : class
{
    private TResult? _result;
    private int? _key;

    public async Task<TResult> GetOrFetch(int key, Func<Task<TResult>> fetchFunc)
    {
        if (_key != key)
        {
            _result = await fetchFunc();
            _key = key;
        }
        return _result!;
    }
}