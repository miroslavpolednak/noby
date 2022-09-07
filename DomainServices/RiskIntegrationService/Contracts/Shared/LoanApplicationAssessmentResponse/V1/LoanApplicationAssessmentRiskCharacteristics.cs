
namespace DomainServices.RiskIntegrationService.Contracts.Shared.V1;

[ProtoContract]
public sealed class LoanApplicationAssessmentRiskCharacteristics
{
    [ProtoMember(1)]
    public AmountDetail? MonthlyIncome { get; set; }

    [ProtoMember(2)]
    public AmountDetail? MonthlyCostsWithoutInstallments { get; set; }

    [ProtoMember(3)]
    public AmountDetail? MonthlyInstallmentsInKB { get; set; }

    [ProtoMember(4)]
    public AmountDetail? MonthlyEntrepreneurInstallmentsInKB { get; set; }

    [ProtoMember(5)]
    public AmountDetail? MonthlyInstallmentsInMPSS { get; set; }

    [ProtoMember(6)]
    public AmountDetail? MonthlyInstallmentsInOFI { get; set; }

    [ProtoMember(7)]
    public AmountDetail? MonthlyInstallmentsInCBCB { get; set; }
}