namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ProtoContract]
public class CustomersExposureCalculateResponse
{
    [ProtoMember(1)]
    public List<CustomersExposureCustomer>? Customers { get; set; }

    [ProtoMember(2)]
    public List<CustomersExposureSummary>? ExposureSummary { get; set; }
}
