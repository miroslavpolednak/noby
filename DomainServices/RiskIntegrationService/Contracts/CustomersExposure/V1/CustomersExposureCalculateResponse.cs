namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;

[ProtoContract]
public class CustomersExposureCalculateResponse
{
    [ProtoMember(1)]
    public List<CustomersExposureCustomer>? Customers { get; set; }

    [ProtoMember(2)]
    public CustomersExposureExposureSummary? ExposureSummary { get; set; }
}
