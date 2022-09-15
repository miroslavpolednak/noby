namespace DomainServices.RiskIntegrationService.Contracts.CustomersExposure.V2;

[ProtoContract]
public class CustomersExposureSummary
{
    [ProtoMember(1)]
    public decimal? TotalExistingExposureKB { get; set; }

    [ProtoMember(2)]
    public decimal? TotalExistingExposureKBNaturalPerson { get; set; }

    [ProtoMember(3)]
    public decimal? TotalExistingExposureKBNonPurpose { get; set; }

    [ProtoMember(4)]
    public decimal? TotalExistingExposureUnsecured { get; set; }
}
