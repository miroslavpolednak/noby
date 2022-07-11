namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase;

[ProtoContract]
public class CreateCaseRequest
    : IRequest<CreateCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public SystemId? LoanApplicationIdMp { get; set; }

    [ProtoMember(2)]
    public string? ResourceProcessIdMp { get; set; }

    [ProtoMember(3)]
    public string? ItChannel { get; set; }
}
