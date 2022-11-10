using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class OfferServiceWrapper : IServiceWrapper
{
    private readonly IOfferServiceAbstraction _offerService;

    public OfferServiceWrapper(IOfferServiceAbstraction offerService)
    {
        _offerService = offerService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.OfferId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.OfferId));

        var result = await _offerService.GetMortgageOfferDetail(input.OfferId.Value, cancellationToken);

        data.Offer = ServiceCallResult.ResolveAndThrowIfError<GetMortgageOfferDetailResponse>(result);

        data.Offer.AdditionalSimulationResults.PaymentScheduleSimple.RemoveAt(2);
    }
}