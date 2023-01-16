using DomainServices.OfferService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class OfferPaymentScheduleServiceWrapper : IServiceWrapper
{
    private readonly IOfferServiceClient _offerService;

    public OfferPaymentScheduleServiceWrapper(IOfferServiceClient offerService)
    {
        _offerService = offerService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.OfferId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.OfferId));

        data.OfferPaymentSchedule = await _offerService.GetMortgageOfferFPSchedule(input.OfferId.Value, cancellationToken);
    }
}