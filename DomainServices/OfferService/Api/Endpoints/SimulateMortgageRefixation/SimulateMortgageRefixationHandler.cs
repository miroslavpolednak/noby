using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationHandler
    : IRequestHandler<SimulateMortgageRefixationRequest, SimulateMortgageRefixationResponse>
{
    public async Task<SimulateMortgageRefixationResponse> Handle(SimulateMortgageRefixationRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
