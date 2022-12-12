using CIS.Core.Results;
using DomainServices.OfferService.Clients;
using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class OfferPaymentScheduleServiceWrapper : IServiceWrapper
{
    private readonly IOfferServiceClients _offerService;

    public OfferPaymentScheduleServiceWrapper(IOfferServiceClients offerService)
    {
        _offerService = offerService;
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        if (!input.OfferId.HasValue)
            throw new ArgumentNullException(nameof(InputParameters.OfferId));

        //var result = await _offerService.GetMortgageOfferFPSchedule(input.OfferId.Value, cancellationToken);

        //data.OfferPaymentSchedule = ServiceCallResult.ResolveAndThrowIfError<GetMortgageOfferFPScheduleResponse>(result);

        var mockPaymentScheduleFull = Enumerable.Range(1, 100)
                                                .Select(i => new PaymentScheduleFull()
                                                {
                                                    PaymentNumber = i.ToString(),
                                                    Date = DateTime.Today.AddMonths(i).ToString()
                                                });

        data.OfferPaymentSchedule = new GetMortgageOfferFPScheduleResponse()
        {
            PaymentScheduleFull =
            {
                mockPaymentScheduleFull
            }
        };
    }
}