namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ServiceContract(Name = "DomainServices.RiskIntegrationService.ICustomerExposureService.V2")]
public interface ICustomerExposureService
{
    ValueTask<CustomerExposureCalculateResponse> Calculate(CustomerExposureCalculateRequest request, CancellationToken cancellationToken = default);
}