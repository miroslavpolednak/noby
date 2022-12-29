namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed record UpdateOfferDocumentIdMediatrRequest(int SalesArrangementId, string OfferDocumentId)
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>
{
}
