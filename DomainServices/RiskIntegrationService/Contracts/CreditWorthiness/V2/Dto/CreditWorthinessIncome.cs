﻿namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessIncome
{
    [ProtoMember(1)]
    public int IncomeTypeId { get; set; }

    [ProtoMember(2)]
    public decimal Amount { get; set; }
}
