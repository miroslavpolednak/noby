namespace DomainServices.RiskIntegrationService.V1
{
    [ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.CreditWorthinessService")]
    public interface ICreditWorthinessService
    {
        ValueTask<Contracts.CreditWorthiness.V1.CreditWorthinessCalculateResponse> Calculate(Contracts.CreditWorthiness.V1.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default);
    }
}
