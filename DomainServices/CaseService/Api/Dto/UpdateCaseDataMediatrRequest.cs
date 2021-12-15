namespace DomainServices.CaseService.Api.Dto;

internal sealed class UpdateCaseDataMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public string ContractNumber { get; init; }

    public UpdateCaseDataMediatrRequest(Contracts.UpdateCaseDataRequest request)
    {
        CaseId = request.CaseId;
        ContractNumber = request.ContractNumber;
    }
}
