namespace DomainServices.CaseService.Api.Dto;

internal sealed class LinkOwnerToCaseMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int UserId { get; init; }

    public LinkOwnerToCaseMediatrRequest(Contracts.LinkOwnerToCaseRequest request)
    {
        CaseId = request.CaseId;
        UserId = request.UserId;
    }
}
