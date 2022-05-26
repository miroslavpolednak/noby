using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetMortgageOfferDetailMediatrRequest
    : IRequest<GetMortgageOfferDetailResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferId { get; init; }

    public GetMortgageOfferDetailMediatrRequest(OfferIdRequest request)
    {
        OfferId = request.OfferId;
    }
}
