namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class CustomersExposureSummary
{
    [ProtoMember(1)]
    public decimal? TotalExistingExposureKB { get; set; }

    [ProtoMember(2)]
    public decimal? TotalExistingExposureKBNaturalPerson { get; set; }
}
