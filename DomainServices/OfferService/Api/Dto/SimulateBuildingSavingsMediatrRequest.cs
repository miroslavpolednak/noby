using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal sealed class SimulateBuildingSavingsMediatrRequest 
    : IRequest<SimulateBuildingSavingsResponse>, CIS.Core.Validation.IValidatableRequest
{
    public SimulateBuildingSavingsRequest Request { get; init; }

    public SimulateBuildingSavingsMediatrRequest(SimulateBuildingSavingsRequest request)
    {
        Request = request;
    }
}
