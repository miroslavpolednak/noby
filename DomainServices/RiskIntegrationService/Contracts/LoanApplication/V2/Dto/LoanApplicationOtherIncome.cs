namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class LoanApplicationOtherIncome
{
    [ProtoMember(1)]
    public int? IncomeOtherTypeId { get; set; }

    [ProtoMember(2)]
    public Shared.AmountDetail? MonthyAmount { get; set; }

    [ProtoMember(3)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(4)]
    public bool IsDomicile { get; set; }

    [ProtoMember(5)]
    public int? ProofTypeId { get; set; }
}
