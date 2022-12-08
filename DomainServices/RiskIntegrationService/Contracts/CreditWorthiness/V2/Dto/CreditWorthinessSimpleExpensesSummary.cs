namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessSimpleExpensesSummary
{
    [ProtoMember(1)]
    public decimal? Rent { get; set; }

    [ProtoMember(2)]
    public decimal? Other { get; set; }
}
