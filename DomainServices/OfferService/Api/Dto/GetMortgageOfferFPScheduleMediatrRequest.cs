using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetMortgageOfferFPScheduleMediatrRequest
    : IRequest<GetMortgageOfferFPScheduleResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferId { get; init; }

    public GetMortgageOfferFPScheduleMediatrRequest(OfferIdRequest request)
    {
        OfferId = request.OfferId;
    }
}
