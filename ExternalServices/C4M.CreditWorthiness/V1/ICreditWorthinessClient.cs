namespace ExternalServices.C4M.CreditWorthiness.V1;

public interface ICreditWorthinessClient
{
    /// <returns><see cref="SuccessfulServiceCallResult{}"/> of type <see cref="Contracts.CreditWorthinessCalculationArguments" /></returns>
    Task<IServiceCallResult> Calculate(Contracts.CreditWorthinessCalculationArguments request, CancellationToken cancellationToken);
}
