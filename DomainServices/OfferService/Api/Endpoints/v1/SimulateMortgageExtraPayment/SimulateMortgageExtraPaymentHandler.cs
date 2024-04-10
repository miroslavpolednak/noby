using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentHandler
    : IRequestHandler<SimulateMortgageExtraPaymentRequest, SimulateMortgageExtraPaymentResponse>
{
    public Task<SimulateMortgageExtraPaymentResponse> Handle(SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
