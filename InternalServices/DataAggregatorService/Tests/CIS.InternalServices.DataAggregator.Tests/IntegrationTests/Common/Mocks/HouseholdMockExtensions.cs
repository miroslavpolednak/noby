using CIS.Foms.Enums;
using CIS.Testing.Common;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common.Mocks;

public static class HouseholdMockExtensions
{
    public static void MockHouseholdList(this IHouseholdServiceClient householdServiceClient, ICustomerOnSAServiceClient customerOnSAServiceClient)
    {
        var fixture = FixtureFactory.Create();

        var householdMain = fixture.Build<Household>().With(h => h.HouseholdId, DefaultMockValues.HouseholdMainId).With(h => h.HouseholdTypeId, (int)HouseholdTypes.Main).Create();
        var householdCodebtor = fixture.Build<Household>().With(h => h.HouseholdId, DefaultMockValues.HouseholdCodebtorId).With(h => h.HouseholdTypeId, (int)HouseholdTypes.Codebtor).Create();

        householdServiceClient.GetHouseholdList(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new List<Household> { householdMain, householdCodebtor });

        var customerOnSaDebtor = CreateCustomer(householdMain.CustomerOnSAId1!.Value, CustomerRoles.Debtor);
        var customerOnSaCodebtor = CreateCustomer(householdMain.CustomerOnSAId2!.Value, CustomerRoles.Debtor);

        customerOnSAServiceClient.GetCustomer(Arg.Is(householdMain.CustomerOnSAId1!.Value), Arg.Any<CancellationToken>()).Returns(customerOnSaDebtor);
        customerOnSAServiceClient.GetCustomer(Arg.Is<int>(id => id != householdMain.CustomerOnSAId1!.Value), Arg.Any<CancellationToken>()).Returns(customerOnSaCodebtor);

        customerOnSAServiceClient.GetCustomerList(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new List<CustomerOnSA>
        {
            customerOnSaDebtor, customerOnSaCodebtor,
            CreateCustomer(householdCodebtor.CustomerOnSAId1!.Value, CustomerRoles.Codebtor),
            CreateCustomer(householdCodebtor.CustomerOnSAId2!.Value, CustomerRoles.Codebtor)
        });

        CustomerOnSA CreateCustomer(int customerSaId, CustomerRoles role)
        {
            var customer = fixture.Build<CustomerOnSA>().With(c => c.CustomerOnSAId, customerSaId).With(c => c.CustomerRoleId, (int)role).Create();
            customer.CustomerIdentifiers.Add(DefaultMockValues.KbIdentity);

            return customer;
        }
    }
}