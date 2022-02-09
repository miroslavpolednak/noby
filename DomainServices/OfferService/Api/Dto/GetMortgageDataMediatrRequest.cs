using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetMortgageDataMediatrRequest 
    : IRequest<GetMortgageDataResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferId { get; init; }

    public GetMortgageDataMediatrRequest(OfferIdRequest request)
    {
        OfferId = request.OfferId;
    }
}
