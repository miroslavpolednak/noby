using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetMortgageDataMediatrRequest 
    : IRequest<GetMortgageDataResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferInstanceId { get; init; }

    public GetMortgageDataMediatrRequest(OfferInstanceIdRequest request)
    {
        OfferInstanceId = request.OfferInstanceId;
    }
}
