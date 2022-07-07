namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[ProtoContract]
public class CalculateRequest
    : IRequest<CalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public string? ResourceProcessIdMp { get; set; }

    [ProtoMember(2)]
    public string? ItChannel { get; set; }

    [ProtoMember(3)]
    public string? RiskBusinessCaseIdMp { get; set; }

    [ProtoMember(4)]
    public HumanUser? HumanUser { get; set; }

    [ProtoMember(5)]
    public LoanApplicationProduct? LoanApplicationProduct { get; set; }

    [ProtoMember(6)]
    public List<LoanApplicationHousehold>? Households { get; set; }
}

