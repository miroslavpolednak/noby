using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Dto;

internal sealed class GetSalesArrangementByOfferIdMediatrRequest
    : IRequest<GetSalesArrangementByOfferIdResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferId { get; init; }

    public GetSalesArrangementByOfferIdMediatrRequest(GetSalesArrangementByOfferIdRequest request)
    {
        OfferId = request.OfferId;
    }
}