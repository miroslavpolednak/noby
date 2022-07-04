using ExternalServices.C4M.CreditWorthiness.V1.Contracts;

namespace ExternalServices.C4M.CreditWorthiness.V1;

internal sealed class MockCreditWorthinessClient
    : ICreditWorthinessClient
{
    public Task<IServiceCallResult> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        var result = new CreditWorthinessCalculation();
        return Task.FromResult((IServiceCallResult)new SuccessfulServiceCallResult<CreditWorthinessCalculation>(result));
    }
}
