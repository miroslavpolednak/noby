using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetOfferInstanceMediatrRequest
    : IRequest<GetOfferInstanceResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferInstanceId { get; init; }

    public GetOfferInstanceMediatrRequest(OfferInstanceIdRequest request)
    {
        OfferInstanceId = request.OfferInstanceId;
    }
}
