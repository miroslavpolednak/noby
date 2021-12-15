namespace DomainServices.CaseService.Api.Dto;

internal sealed class UpdateCaseStateMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int State { get; init; }

    public UpdateCaseStateMediatrRequest(Contracts.UpdateCaseStateRequest request)
    {
        CaseId = request.CaseId;
        State = request.State;
    }
}
