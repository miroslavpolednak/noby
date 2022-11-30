namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public sealed class CreditWorthinessSimpleObligationsSummary
{
    [ProtoMember(1)]
    public decimal? CreditCardsAmount { get; set; }

    [ProtoMember(2)]
    public decimal? AuthorizedOverdraftsAmount { get; set; }

    [ProtoMember(3)]
    public decimal? LoansInstallmentsAmount { get; set; }
}
