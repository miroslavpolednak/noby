namespace DomainServices.RiskIntegrationService.V1
{
    [ServiceContract(Name = "DomainServices.RiskIntegrationService.v1.ICustomersExposureService")]
    public interface ICustomersExposureService
    {
        ValueTask<Contracts.CustomersExposure.V1.CustomersExposureCalculateResponse> Calculate(Contracts.CustomersExposure.V1.CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default);
    }
}