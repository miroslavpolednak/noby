using DomainServices.OfferService.Clients;

namespace CIS.InternalServices.DataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class OfferServiceWrapper : IServiceWrapper
{
    private readonly IOfferServiceClient _offerService;

    public OfferServiceWrapper(IOfferServiceClient offerService)
    {
        _offerService = offerService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.OfferId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.OfferId));

        data.Offer = await _offerService.GetMortgageOfferDetail(input.OfferId.Value, cancellationToken);
    }
}