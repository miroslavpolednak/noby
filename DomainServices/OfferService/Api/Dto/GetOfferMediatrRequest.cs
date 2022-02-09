using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetOfferMediatrRequest
    : IRequest<GetOfferResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferId { get; init; }

    public GetOfferMediatrRequest(OfferIdRequest request)
    {
        OfferId = request.OfferId;
    }
}
