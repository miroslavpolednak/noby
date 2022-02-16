namespace CIS.Core.Results;

public class SuccessfulServiceCallResult : IServiceCallResult
{
    public bool Success => true;
}

public sealed class SuccessfulServiceCallResult<TModel> 
    : SuccessfulServiceCallResult
{
    public TModel Model { get; init; }
    
    public SuccessfulServiceCallResult(TModel model)
    {
        Model = model;
    }
}
