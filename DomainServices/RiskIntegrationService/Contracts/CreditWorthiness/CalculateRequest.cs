namespace DomainServices.RiskIntegrationService.Contracts.CreditWorthiness;

[DataContract]
public class CalculateRequest
    : IRequest<CalculateResponse>, CIS.Core.Validation.IValidatableRequest
{
    [DataMember(Order = 1)]
    public string? ResourceProcessIdMp { get; set; }

    [DataMember(Order = 2)]
    public string? ItChannel { get; set; }

    [DataMember(Order = 3)]
    public string? RiskBusinessCaseIdMp { get; set; }

    [DataMember(Order = 4)]
    public HumanUser? HumanUser { get; set; }

    [DataMember(Order = 5)]
    public LoanApplicationProduct? LoanApplicationProduct { get; set; }

    [DataMember(Order = 6)]
    public List<LoanApplicationHousehold>? Households { get; set; }
}

