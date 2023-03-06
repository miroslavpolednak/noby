using DomainServices.OfferService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class OfferServiceWrapper : IServiceWrapper
{
    private readonly IOfferServiceClient _offerService;

    public OfferServiceWrapper(IOfferServiceClient offerService)
    {
        _offerService = offerService;
    }

    public DataSource DataSource => DataSource.OfferService;

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateOfferId();

        data.Offer = await _offerService.GetMortgageOfferDetail(input.OfferId!.Value, cancellationToken);
    }

    public async Task LoadPaymentSchedule(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateOfferId();
        
        data.OfferPaymentSchedule = await _offerService.GetMortgageOfferFPSchedule(input.OfferId!.Value, cancellationToken);
    }
}