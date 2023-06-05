namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public sealed class LoanApplicationRentIncome
{
    [ProtoMember(1)]
    public bool IsDomicile { get; set; }

    [ProtoMember(2)]
    public int? ProofTypeId { get; set; }

    [ProtoMember(3)]
    public Shared.AmountDetail? MonthlyIncomeAmount { get; set; }
}
