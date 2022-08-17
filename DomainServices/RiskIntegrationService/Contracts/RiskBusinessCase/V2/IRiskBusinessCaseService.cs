namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.RiskBusinessCaseService.V2")]
public interface IRiskBusinessCaseService
{
    ValueTask<CreateCaseResponse> CreateCase(CreateCaseRequest request, CancellationToken cancellationToken = default);
}
