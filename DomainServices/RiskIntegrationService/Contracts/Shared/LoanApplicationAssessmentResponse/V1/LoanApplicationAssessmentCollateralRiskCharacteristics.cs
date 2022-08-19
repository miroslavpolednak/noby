
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentCollateralRiskCharacteristics
{
    [ProtoMember(1)]
    public long? Ltv { get; set; }

    [ProtoMember(2)]
    public long? Ltfv { get; set; }

    [ProtoMember(3)]
    public long? Ltp { get; set; }

    [ProtoMember(4)]
    public long? SumAppraisedValue { get; set; }
}
