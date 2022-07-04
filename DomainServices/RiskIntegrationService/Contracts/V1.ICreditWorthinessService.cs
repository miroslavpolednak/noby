namespace DomainServices.RiskIntegrationService.v1;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.CreditWorthinessService")]
public interface ICreditWorthinessService
{
    ValueTask<Contracts.CreditWorthiness.CalculateResponse> Calculate(Contracts.CreditWorthiness.CalculateRequest request, CancellationToken cancellationToken = default);
}
