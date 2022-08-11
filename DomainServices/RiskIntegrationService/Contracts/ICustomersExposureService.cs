namespace DomainServices.RiskIntegrationService.V2
{
    [ServiceContract(Name = "DomainServices.RiskIntegrationService.v2.ICustomersExposureService")]
    public interface ICustomersExposureService
    {
        ValueTask<Contracts.CustomersExposure.V2.CustomersExposureCalculateResponse> Calculate(Contracts.CustomersExposure.V2.CustomersExposureCalculateRequest request, CancellationToken cancellationToken = default);
    }
}