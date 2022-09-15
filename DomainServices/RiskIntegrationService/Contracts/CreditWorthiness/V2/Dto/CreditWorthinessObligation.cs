namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessObligation
{
    [ProtoMember(1)]
    public int ObligationTypeId { get; set; }

    [ProtoMember(2)]
    public decimal? Amount { get; set; }

    [ProtoMember(3)]
    public decimal? AmountConsolidated { get; set; }

    [ProtoMember(4)]
    public decimal? Installment { get; set; }

    [ProtoMember(5)]
    public decimal? InstallmentConsolidated { get; set; }

    [ProtoMember(6)]
    public bool IsObligationCreditorExternal { get; set; }
}
