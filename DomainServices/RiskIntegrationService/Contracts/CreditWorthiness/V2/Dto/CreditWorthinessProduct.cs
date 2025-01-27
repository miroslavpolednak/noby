﻿namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessProduct
{
    [ProtoMember(1)]
    public int ProductTypeId { get; set; }

    [ProtoMember(2)]
    public int LoanDuration { get; set; }

    [ProtoMember(3)]
    public decimal LoanInterestRate { get; set; }

    [ProtoMember(4)]
    public int LoanAmount { get; set; }

    [ProtoMember(5)]
    public int LoanPaymentAmount { get; set; }

    [ProtoMember(6)]
    public int FixedRatePeriod { get; set; }
}
