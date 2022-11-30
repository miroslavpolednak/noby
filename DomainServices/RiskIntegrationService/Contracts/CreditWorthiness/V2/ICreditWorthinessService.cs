namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.CreditWorthinessService.V2")]
public interface ICreditWorthinessService
{
    ValueTask<CreditWorthinessCalculateResponse> Calculate(CreditWorthinessCalculateRequest request, CancellationToken cancellationToken = default);

    ValueTask<CreditWorthinessSimpleCalculateResponse> SimpleCalculate(CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken = default);
}
