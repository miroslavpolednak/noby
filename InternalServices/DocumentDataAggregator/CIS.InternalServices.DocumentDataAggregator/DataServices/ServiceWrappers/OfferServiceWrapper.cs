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

        var fixture = new Fixture();

        fixture.Customize<GrpcDate>(x => x.FromFactory(() => fixture.Create<DateTime>()).OmitAutoProperties());
        fixture.Customize<NullableGrpcDate>(x => x.FromFactory(() => fixture.Create<DateTime>()!).OmitAutoProperties());

        data.Offer = fixture.Build<GetMortgageOfferDetailResponse>()
                            .With(c => c.OfferId, input.OfferId)
                            .Create();

        data.Offer.AdditionalSimulationResults.PaymentScheduleSimple.Add(fixture.Create<PaymentScheduleSimple>());

        data.Offer.AdditionalSimulationResults.Fees.Add(fixture.Create<ResultFee>());

        data.Offer.AdditionalSimulationResults.MarketingActions.Add(new ResultMarketingAction
        {
            Applied = 1,
            Code = "DOMICILACE"
        });
    }

    public class TestFixture
    {
        public string Name { get; set; }

        public ICollection<GrpcDate> Test { get; set; }
    }
}