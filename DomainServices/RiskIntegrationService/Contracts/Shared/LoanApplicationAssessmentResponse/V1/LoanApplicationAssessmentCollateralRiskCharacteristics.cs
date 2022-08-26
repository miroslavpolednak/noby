
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentCollateralRiskCharacteristics
{
    [ProtoMember(1)]
    public decimal? Ltv { get; set; }

    [ProtoMember(2)]
    public decimal? Ltfv { get; set; }

    [ProtoMember(3)]
    public decimal? Ltp { get; set; }

    [ProtoMember(4)]
    public long? SumAppraisedValue { get; set; }

    [ProtoMember(5)]
    public long? TotalUsedValue { get; set; }
}
