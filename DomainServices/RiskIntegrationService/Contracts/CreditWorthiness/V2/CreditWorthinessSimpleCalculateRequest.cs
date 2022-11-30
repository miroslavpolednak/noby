namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract]
public class CreditWorthinessSimpleCalculateRequest
    : IRequest<CreditWorthinessSimpleCalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public string ResourceProcessId { get; set; } = default!;

    [ProtoMember(2)]
    public Shared.Identity? UserIdentity { get; set; }

    [ProtoMember(3)]
    public CreditWorthinessProduct? Product { get; set; }

    [ProtoMember(4)]
    public int ChildrenCount { get; set; }

    [ProtoMember(5)]
    public CreditWorthinessSimpleExpensesSummary? ExpensesSummary { get; set; }

    [ProtoMember(6)]
    public CreditWorthinessSimpleObligationsSummary? ObligationsSummary { get; set; }

    [ProtoMember(7)]
    public string? PrimaryCustomerId { get; set; }

    [ProtoMember(8)]
    public decimal? TotalMonthlyIncome { get; set; }
}

