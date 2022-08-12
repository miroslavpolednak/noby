namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.ICustomersExposureService.V2")]
public interface ICustomersExposureService
{
    ValueTask<CustomersExposureCalculateResponse> Calculate(CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default);
}