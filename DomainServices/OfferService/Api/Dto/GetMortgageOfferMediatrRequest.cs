using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetMortgageOfferMediatrRequest
    : IRequest<GetMortgageOfferResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferId { get; init; }

    public GetMortgageOfferMediatrRequest(OfferIdRequest request)
    {
        OfferId = request.OfferId;
    }
}
