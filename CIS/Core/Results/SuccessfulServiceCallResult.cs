namespace CIS.Core.Results
{
    public sealed class SuccessfulServiceCallResult : IServiceCallResult
    {
        public bool Success => true;
    }

    public sealed class SuccessfulServiceCallResult<TModel> 
        : IServiceCallResult
    {
        public TModel Model { get; init; }
        public bool Success => true;

        public SuccessfulServiceCallResult(TModel model)
        {
            Model = model;
        }
    }
}
