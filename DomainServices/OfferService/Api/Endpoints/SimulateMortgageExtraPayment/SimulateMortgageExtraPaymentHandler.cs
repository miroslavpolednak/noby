using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Endpoints.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentHandler
    : IRequestHandler<SimulateMortgageExtraPaymentRequest, SimulateMortgageExtraPaymentResponse>
{
    public async Task<SimulateMortgageExtraPaymentResponse> Handle(SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
