using SharedTypes.Enums;
using SharedTypes.GrpcTypes;
using CIS.Testing.Common;
using DomainServices.CustomerService.Clients.v1;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class CustomerMockExtensions
{
    public static void MockCustomerList(this ICustomerServiceClient customerServiceClient)
    {
        var fixture = FixtureFactory.Create();

        customerServiceClient.GetCustomerList(Arg.Any<IEnumerable<Identity>>(), Arg.Any<CancellationToken>()).Returns(args =>
        {
            var customers = args.Arg<IEnumerable<Identity>>().Select(identity =>
            {
                var customer = fixture.Create<DomainServices.CustomerService.Contracts.Customer>();
                
                customer.Identities.Add(new Identity(identity.IdentityId, IdentitySchemes.Kb));

                return customer;
            });

            return new DomainServices.CustomerService.Contracts.CustomerListResponse { Customers = { customers } };
        });
    }
}