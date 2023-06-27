using CIS.Testing.Common;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class SalesArrangementMockExtensions
{
    public static void MockGetSalesArrangement(this ISalesArrangementServiceClient salesArrangementServiceClient) => 
        salesArrangementServiceClient.MockGetSalesArrangement<object>(delegate { });

    public static void MockGetSalesArrangement<TParameter>(this ISalesArrangementServiceClient salesArrangementServiceClient, Action<SalesArrangement, TParameter> parameterSetter)
    {
        var fixture = FixtureFactory.Create();

        var salesArrangement = fixture.Build<SalesArrangement>().With(s => s.SalesArrangementTypeId, 1).Create();

        //rewrite whatever was set in the OneOf reference
        parameterSetter(salesArrangement, fixture.Create<TParameter>());

        salesArrangementServiceClient.GetSalesArrangement(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(salesArrangement);
    }
}