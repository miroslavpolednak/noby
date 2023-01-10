namespace CIS.Core.Results;

public static class ServiceCallResult
{
    public static TModel ResolveAndThrowIfError<TModel>(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<TModel> r => r.Model,
            EmptyServiceCallResult => throw new Exceptions.CisValidationException(0, $"ServiceCallResult is empty but should be instance of {typeof(TModel)}"),
            _ => throw new NotImplementedException("ServiceCallResult type unknown (Resolve<>)")
        };
}
