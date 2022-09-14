namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;

[ProtoContract, CompatibilityLevel(CompatibilityLevel.Level300)]
public class CreditWorthinessCalculateRequest
    : IRequest<CreditWorthinessCalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public string? ResourceProcessId { get; set; }

    [ProtoMember(3)]
    public string? RiskBusinessCaseId { get; set; }

    [ProtoMember(4)]
    public Shared.Identity? UserIdentity { get; set; }

    [ProtoMember(5)]
    public CreditWorthinessProduct? Product { get; set; }

    [ProtoMember(6)]
    public List<CreditWorthinessHousehold>? Households { get; set; }
}

