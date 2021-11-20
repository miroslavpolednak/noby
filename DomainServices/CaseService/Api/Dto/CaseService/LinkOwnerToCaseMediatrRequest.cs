namespace DomainServices.CaseService.Api.Dto.CaseService;

internal sealed class LinkOwnerToCaseMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public long CaseId { get; init; }
    public int PartyId { get; init; }

    public LinkOwnerToCaseMediatrRequest(Contracts.LinkOwnerToCaseRequest request)
    {
        CaseId = request.CaseId;
        PartyId = request.PartyId;
    }
}
