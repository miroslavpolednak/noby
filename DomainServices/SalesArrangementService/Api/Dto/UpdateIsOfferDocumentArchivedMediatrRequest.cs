namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed record UpdateIsOfferDocumentArchivedMediatrRequest(int SalesArrangementId, bool IsOfferDocumentArchived)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
