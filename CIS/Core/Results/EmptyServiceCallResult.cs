namespace CIS.Core.Results;

public sealed class EmptyServiceCallResult : IServiceCallResult
{
    public bool Success => true;
}