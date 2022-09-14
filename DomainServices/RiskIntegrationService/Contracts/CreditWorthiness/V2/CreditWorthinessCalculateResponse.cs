using DomainServices.RiskIntegrationService.Contracts.Shared;

namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class CreditWorthinessCalculateResponse
{
    [ProtoMember(1)]
    public long? InstallmentLimit { get; set; }

    [ProtoMember(2)]
    public long? MaxAmount { get; set; }

    [ProtoMember(3)]
    public long? RemainsLivingAnnuity { get; set; }

    [ProtoMember(4)]
    public long? RemainsLivingInst { get; set; }

    [ProtoMember(5)]
    public decimal? Dti { get; set; }

    [ProtoMember(6)]
    public decimal? Dsti { get; set; }

    [ProtoMember(7)]
    public CreditWorthinessResults WorthinessResult { get; set; }

    [ProtoMember(8)]
    public ResultReasonDetail? ResultReason { get; set; }
}
