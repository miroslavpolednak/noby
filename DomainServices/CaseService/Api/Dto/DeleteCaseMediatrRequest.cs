namespace DomainServices.CaseService.Api.Dto;

internal sealed class DeleteCaseMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; set; }

    public DeleteCaseMediatrRequest(Contracts.DeleteCaseRequest request)
    {
        CaseId = request.CaseId;
    }
}
