namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V1;

[ProtoContract]
public class CustomersExposureExposureSummary
{
    [ProtoMember(1)]
    public decimal? TotalExistingExposureKB { get; set; }

    [ProtoMember(2)]
    public decimal? TotalExistingExposureKBNaturalPerson { get; set; }
}
