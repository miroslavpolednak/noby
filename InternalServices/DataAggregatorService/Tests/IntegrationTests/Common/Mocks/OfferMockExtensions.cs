using CIS.Testing.Common;
using DomainServices.OfferService.Clients.v1;
using DomainServices.OfferService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class OfferMockExtensions
{
    public static void MockGetOfferDetail(this IOfferServiceClient offerServiceClient)
    {
        var fixture = FixtureFactory.Create();

        fixture.Behaviors.Add(new RepeatedFieldBehavior());

        var offer = fixture.Build<GetMortgageDetailResponse>().Create();
        var fullPaymentSchedule = fixture.Create<GetMortgageOfferFPScheduleResponse>();

        offerServiceClient.GetMortgageDetail(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(offer);
        offerServiceClient.GetMortgageOfferFPSchedule(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(fullPaymentSchedule);
    }
}