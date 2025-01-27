﻿namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessCustomer
{
    [ProtoMember(1)]
    public string? PrimaryCustomerId { get; set; }

    [ProtoMember(2)]
    public bool HasPartner { get; set; }

    [ProtoMember(3)]
    public int? MaritalStateId { get; set; }

    [ProtoMember(4)]
    public List<CreditWorthinessIncome>? Incomes { get; set; }

    [ProtoMember(5)]
    public List<CreditWorthinessObligation>? Obligations { get; set; }
}
