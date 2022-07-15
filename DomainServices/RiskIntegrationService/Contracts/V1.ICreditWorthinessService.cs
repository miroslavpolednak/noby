namespace DomainServices.RiskIntegrationService.v1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.CreditWorthinessService")]
public interface ICreditWorthinessService
{
    ValueTask<Contracts.CreditWorthiness.CreditWorthinessCalculateResponse> Calculate(Contracts.CreditWorthiness.CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default);
}
