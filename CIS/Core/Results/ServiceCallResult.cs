namespace CIS.Core.Results;

public static class ServiceCallResult
{
    public static TModel ResolveAndThrowIfError<TModel>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<TModel> r => r.Model,
            _ => throw new NotImplementedException("ServiceCallResult type unknown (Resolve<>)")
        };
}
