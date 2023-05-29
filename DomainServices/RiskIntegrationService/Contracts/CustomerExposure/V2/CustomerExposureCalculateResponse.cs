namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ProtoContract]
public class CustomerExposureCalculateResponse
{
    [ProtoMember(1)]
    public List<CustomerExposureCustomer>? Customers { get; set; }

    [ProtoMember(2)]
    public CustomerExposureSummary? ExposureSummary { get; set; }
}
