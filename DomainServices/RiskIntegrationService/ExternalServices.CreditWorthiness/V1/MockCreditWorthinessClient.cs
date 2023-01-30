using DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1.Contracts;

namespace DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V1;

internal sealed class MockCreditWorthinessClient
    : ICreditWorthinessClient
{
    public Task<CreditWorthinessCalculation> Calculate(CreditWorthinessCalculationArguments request, CancellationToken cancellationToken)
    {
        return Task.FromResult<CreditWorthinessCalculation>(new CreditWorthinessCalculation
        {
            InstallmentLimit = 1000,
            MaxAmount = 1000000,
            RemainsLivingAnnuity = 2000,
            RemainsLivingInst = 1500,
            ResultReason = new ResultReason
            {
                Code = "a",
                Description = "b"
            }
        });
    }
}
