namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public class LoanApplicationLimit
{
    [ProtoMember(1)]
    public Amount? LoanApplicationLimitX { get; set; }

    [ProtoMember(2)]
    public Amount? LoanApplicationInstallmentLimit { get; set; }

    [ProtoMember(3)]
    public Amount? LoanApplicationCollateralLimit { get; set; }

    [ProtoMember(4)]
    public Amount? RemainingAnnuityLivingAmount { get; set; }

    [ProtoMember(5)]
    public bool? CalculationIrStressed { get; set; }

    [ProtoMember(6)]
    public long? Iir { get; set; }

    [ProtoMember(7)]
    public long? Cir { get; set; }

    [ProtoMember(8)]
    public long? Dti { get; set; }

    [ProtoMember(9)]
    public long? Dsti { get; set; }
}