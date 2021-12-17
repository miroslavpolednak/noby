using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal class LinkModelationToSalesArrangementMediatrRequest
    : IRequest<Google.Protobuf.WellKnownTypes.Empty>, CIS.Core.Validation.IValidatableRequest
{
    public int SalesArrangementId { get; init; }
    public int OfferInstanceId { get; init; }

    public LinkModelationToSalesArrangementMediatrRequest(LinkModelationToSalesArrangementRequest model)
    {
        SalesArrangementId = model.SalesArrangementId;
        OfferInstanceId = model.OfferInstanceId;
    }
}
