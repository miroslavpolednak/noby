namespace DomainServices.RiskIntegrationService.Contracts.Shared.ExpensesSummary.V1;

[ProtoContract]
public class ExpensesSummary
{
    [ProtoMember(1)]
    public decimal? Rent { get; set; }

    [ProtoMember(2)]
    public decimal? Saving { get; set; }

    [ProtoMember(3)]
    public decimal? Insurance { get; set; }

    [ProtoMember(4)]
    public decimal? Other { get; set; }
}
