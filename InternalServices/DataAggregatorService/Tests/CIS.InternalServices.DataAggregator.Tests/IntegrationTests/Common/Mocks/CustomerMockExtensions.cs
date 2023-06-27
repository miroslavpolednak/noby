using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.Testing.Common;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class CustomerMockExtensions
{
    public static void MockCustomerList(this ICustomerServiceClient customerServiceClient)
    {
        var customer = FixtureFactory.Create().Create<CustomerDetailResponse>();
        customer.Identities.Add(new Identity(0, IdentitySchemes.Kb));

        customerServiceClient.GetCustomerList(Arg.Any<IEnumerable<Identity>>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new CustomerListResponse { Customers = { customer } });
    }
}