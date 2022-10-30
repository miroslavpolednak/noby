using AutoFixture;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.DataServices.ServiceWrappers;

[TransientService, SelfService]
internal class OfferServiceWrapper : IServiceWrapper
{
    //private readonly IOfferServiceAbstraction _offerService;

    //public OfferServiceWrapper(IOfferServiceAbstraction offerService)
    //{
    //    _offerService = offerService;
    //}

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        //var result = await _offerService.GetMortgageOfferDetail(offerId, cancellationToken);

        //return ServiceCallResult.ResolveAndThrowIfError<GetMortgageOfferDetailResponse>(result);

        data.Offer = new Fixture().Build<GetMortgageOfferDetailResponse>()
                                  .With(c => c.OfferId, input.OfferId)
                                  .Create();

        data.Offer.SimulationInputs.ExpectedDateOfDrawing = new NullableGrpcDate(1980, 10, 25);

        data.Offer.AdditionalSimulationResults.MarketingActions.Add(new ResultMarketingAction
        {
            Applied = 1,
            Code = "DOMICILACE"
        });

        data.OfferCustom = new OfferCustomData();
    }
}