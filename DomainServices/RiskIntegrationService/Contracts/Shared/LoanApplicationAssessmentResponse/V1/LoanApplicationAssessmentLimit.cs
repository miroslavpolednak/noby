
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public class LoanApplicationAssessmentLimit
{
    [ProtoMember(1)]
    public AmountDetail? Limit { get; set; }

    [ProtoMember(2)]
    public AmountDetail? InstallmentLimit { get; set; }

    [ProtoMember(3)]
    public AmountDetail? CollateralLimit { get; set; }

    [ProtoMember(4)]
    public AmountDetail? RemainingAnnuityLivingAmount { get; set; }

    [ProtoMember(5)]
    public bool IsCalculationStressed { get; set; }

    [ProtoMember(6)]
    public long? Iir { get; set; }

    [ProtoMember(7)]
    public long? Cir { get; set; }

    [ProtoMember(8)]
    public decimal? Dti { get; set; }

    [ProtoMember(9)]
    public long? Dsti { get; set; }
}