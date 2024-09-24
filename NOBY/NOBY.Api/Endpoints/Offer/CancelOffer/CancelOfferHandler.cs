using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.CancelOffer;

internal sealed class CancelOfferHandler(IOfferServiceClient _offerService) : IRequestHandler<CancelOfferRequest>
{
    public async Task Handle(CancelOfferRequest request, CancellationToken cancellationToken)
    {
        var offer = await _offerService.GetOffer(request.OfferId, cancellationToken);

        if (offer.Data.OfferType is not OfferTypes.MortgageRefixation)
        {
            throw new NobyValidationException(90032, "Incorrect offer type, offer type has to be MortgageRefixation");
        }

        // Has communicated flag?
        if ((offer.Data.Flags & (int)EnumOfferFlagTypes.Communicated) == (int)EnumOfferFlagTypes.Communicated)
        {
            await _offerService.UpdateOffer(new()
            {
                OfferId = request.OfferId,
                CaseId = request.CaseId,
                RemoveIsCommunicatedFlag = true
            }, cancellationToken);
        }

        await _offerService.CancelOffer(request.OfferId, cancellationToken);
    }
}
