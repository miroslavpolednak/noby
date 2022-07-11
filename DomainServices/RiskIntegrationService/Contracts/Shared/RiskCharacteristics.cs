namespace DomainServices.RiskIntegrationService.Contracts;

[ProtoContract]
public sealed class RiskCharacteristics
{
    [ProtoMember(1)]
    public Amount? MonthlyIncomeAmount { get; set; }

    [ProtoMember(2)]
    public Amount? MonthlyCostsWithoutInstAmount { get; set; }

    [ProtoMember(3)]
    public Amount? MonthlyInstallmentsInKBAmount { get; set; }

    [ProtoMember(4)]
    public Amount? MonthlyEntrepreneurInstallmentsInKBAmount { get; set; }

    [ProtoMember(5)]
    public Amount? MonthlyInstallmentsInMPSSAmount { get; set; }

    [ProtoMember(6)]
    public Amount? MonthlyInstallmentsInOFIAmount { get; set; }

    [ProtoMember(7)]
    public Amount? MonthlyInstallmentsInCBCBAmount { get; set; }
}