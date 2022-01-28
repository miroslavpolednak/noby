namespace CIS.Core.Results;

public static class ServiceCallResult
{
    public static bool Resolve(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult => true,
            _ => throw new NotImplementedException()
        };

    public static TModel Resolve<TModel>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<TModel> r => r.Model,
            _ => throw new NotImplementedException()
        };

    public static TModel? ResolveToDefault<TModel>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<TModel> r => r.Model,
            _ => default(TModel)
        };
}
