namespace DomainServices.RiskIntegrationService.V2
{
    [ServiceContract(Name = "DomainServices.RiskIntegrationService.v2.CreditWorthinessService")]
    public interface ICreditWorthinessService
    {
        ValueTask<Contracts.CreditWorthiness.V2.CreditWorthinessCalculateResponse> Calculate(Contracts.CreditWorthiness.V2.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default);
    }
}
