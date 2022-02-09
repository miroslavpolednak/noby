using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Dto;

internal sealed class SimulateMortgageMediatrRequest 
    : IRequest<SimulateMortgageResponse>, CIS.Core.Validation.IValidatableRequest
{
    public SimulateMortgageRequest Request { get; init; }

    public SimulateMortgageMediatrRequest(SimulateMortgageRequest request)
    {
        Request = request;
    }
}
