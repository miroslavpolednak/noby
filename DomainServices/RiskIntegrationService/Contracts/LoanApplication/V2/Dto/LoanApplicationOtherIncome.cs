﻿namespace DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;

[ProtoContract]
public class LoanApplicationOtherIncome
{
    [ProtoMember(1)]
    public int IncomeOtherTypeId { get; set; }

    [ProtoMember(2)]
    public Shared.AmountDetail MonthlyIncomeAmount { get; set; } = null!;

    [ProtoMember(3)]
    public Shared.BankAccountDetail? BankAccount { get; set; }

    [ProtoMember(4)]
    public bool IsDomicile { get; set; }

    [ProtoMember(5)]
    public int? ProofTypeId { get; set; }
}
