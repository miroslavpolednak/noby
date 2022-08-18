namespace DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;

[ProtoContract]
public class CreateCaseRequest
    : IRequest<CreateCaseResponse>, CIS.Core.Validation.IValidatableRequest
{
    [ProtoMember(1)]
    public long SalesArrangementId { get; set; }

    [ProtoMember(2)]
    public string? ResourceProcessId { get; set; }
}
