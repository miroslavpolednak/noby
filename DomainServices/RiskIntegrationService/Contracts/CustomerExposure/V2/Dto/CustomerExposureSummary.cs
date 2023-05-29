namespace DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;

[ProtoContract]
public class CustomerExposureSummary
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
