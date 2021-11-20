using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal partial class GetBuildingSavingsDataMediatrRequest 
    : IRequest<GetBuildingSavingsDataResponse>, CIS.Core.Validation.IValidatableRequest
{
    public int OfferInstanceId { get; init; }

    public GetBuildingSavingsDataMediatrRequest(OfferInstanceIdRequest request)
    {
        OfferInstanceId = request.OfferInstanceId;
    }
}
