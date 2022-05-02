namespace CIS.Core.Results;

public static class ServiceCallResult
{
    public static bool Resolve(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            ErrorServiceCallResult => false,
            _ => throw new NotImplementedException("ServiceCallResult type unknown (Resolve)")
        };

    public static TModel ResolveAndThrowIfError<TModel>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<TModel> r => r.Model,
            EmptyServiceCallResult => throw new Exceptions.CisArgumentNullException(0, $"ServiceCallResult is empty but should be instance of {typeof(TModel)}", nameof(result)),
            ErrorServiceCallResult r2 => throw new Exceptions.ServiceCallResultErrorException(r2),
            _ => throw new NotImplementedException("ServiceCallResult type unknown (Resolve<>)")
        };

    public static TModel? ResolveToDefault<TModel>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<TModel> r => r.Model,
            _ => default(TModel)
        };

    public static bool IsEmptyResult(IServiceCallResult result) =>
        result switch
        {
            EmptyServiceCallResult => true,
            _ => false
        };
    
    public static bool IsSuccessResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            EmptyServiceCallResult => true,
            _ => false
        };
}
