using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;

namespace NOBY.Api.Endpoints.Offer.SetOfferFlags;

internal sealed class SetOfferFlagsHandler(IOfferServiceClient _offerService)
        : IRequestHandler<SetOfferFlagsRequest>
{
    public async Task Handle(SetOfferFlagsRequest request, CancellationToken cancellationToken)
    {
        var offerInstance = await _offerService.GetOffer(request.OfferId, cancellationToken);

        // aktualni nastaveni flagu
        OfferFlagTypes flag = (OfferFlagTypes)offerInstance.Data.Flags;
        
        // nastavit flags nove
        foreach (var flagToSet in request.Flags!)
        {
            if (flag.HasFlag(flagToSet.FlagType) && !flagToSet.SetFlag)
            {
                flag ^= flagToSet.FlagType;
            }
            else if (!flag.HasFlag(flagToSet.FlagType) && flagToSet.SetFlag)
            {
                flag |= flagToSet.FlagType;

                if (flagToSet.FlagType == OfferFlagTypes.Selected)
                {
                    await validateSelectedOffer(offerInstance, cancellationToken);
                }
            }
            // ostatni ignorovat
        }

        var updateRequest = new UpdateOfferRequest
        {
            OfferId = request.OfferId,
            Flags = (int)flag
        };
        await _offerService.UpdateOffer(updateRequest, cancellationToken);
    }

    private async Task validateSelectedOffer(GetOfferResponse offerInstance, CancellationToken cancellationToken)
    {
        var list = await _offerService.GetOfferList(offerInstance.Data.CaseId!.Value, offerInstance.Data.OfferType, true, cancellationToken);

        if (list.Any(t => t.Data.OfferId != offerInstance.Data.OfferId
            && ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Selected)
            && ((OfferFlagTypes)t.Data.Flags).HasFlag(OfferFlagTypes.Communicated)))
        {
            throw new NobyValidationException("Selected offer already exist");
        }
    }
}
