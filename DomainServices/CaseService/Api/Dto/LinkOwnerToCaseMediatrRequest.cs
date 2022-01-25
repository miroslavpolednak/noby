namespace DomainServices.CaseService.Api.Dto;

internal sealed class LinkOwnerToCaseMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public int CaseOwnerUserId { get; init; }
    public long CaseId { get; init; }

    public LinkOwnerToCaseMediatrRequest(Contracts.LinkOwnerToCaseRequest request)
    {
        CaseId = request.CaseId;
        CaseOwnerUserId = request.CaseOwnerUserId;
    }
}
