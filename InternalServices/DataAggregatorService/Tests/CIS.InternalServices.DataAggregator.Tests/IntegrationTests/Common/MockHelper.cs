using AutoFixture;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.Testing.Common;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.DocumentOnSAService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.OfferService.Clients;
using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.Common;

public static class MockHelper
{
    public static void MockGetSalesArrangement(this ISalesArrangementServiceClient salesArrangementServiceClient)
    {
        var fixture = FixtureFactory.Create();

        var salesArrangement = fixture.Build<SalesArrangement>().With(s => s.SalesArrangementTypeId, 1).Create();
        
        //rewrite whatever was set in the OneOf reference
        salesArrangement.Mortgage = fixture.Create<SalesArrangementParametersMortgage>();

        salesArrangementServiceClient.GetSalesArrangement(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(salesArrangement);
    }

    public static void MockCustomerList(this ICustomerServiceClient customerServiceClient)
    {
        var customer = FixtureFactory.Create().Create<CustomerDetailResponse>();
        customer.Identities.Add(new Identity(0, IdentitySchemes.Kb));

        customerServiceClient.GetCustomerList(Arg.Any<IEnumerable<Identity>>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(new CustomerListResponse { Customers = { customer } });
    }

    public static void MockDocumentOnSa(this IDocumentOnSAServiceClient documentOnSAServiceClient)
    {
        var fixture = FixtureFactory.Create();

        var documentOnSa4 = fixture.Build<DocumentOnSAToSign>().With(d => d.DocumentTypeId, 4).With(d => d.IsFinal, true).Create();
        var documentOnSa5 = fixture.Build<DocumentOnSAToSign>().With(d => d.DocumentTypeId, 5).With(d => d.IsFinal, true).Create();

        var response = new GetDocumentsOnSAListResponse { DocumentsOnSA = { new[] { documentOnSa4, documentOnSa5 } } };

        documentOnSAServiceClient.GetDocumentsOnSAList(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(response);
    }

    public static void MockGetOfferDetail(this IOfferServiceClient offerServiceClient)
    {
        var offer = FixtureFactory.Create().Create<GetMortgageOfferDetailResponse>();

        offer.AdditionalSimulationResults.PaymentScheduleSimple.Add(Enumerable.Range(1, 3).Select(i => new PaymentScheduleSimple { PaymentNumber = i.ToString()}));

        offerServiceClient.GetMortgageOfferDetail(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(offer);

        var fullPaymentSchedule = new GetMortgageOfferFPScheduleResponse
        {
            PaymentScheduleFull =
            {
                Enumerable.Range(1, 3).Select(i => i.ToString()).Select(i => new PaymentScheduleFull { Amount = i, PaymentNumber = i })
            }
        };

        offerServiceClient.GetMortgageOfferFPSchedule(Arg.Any<int>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(fullPaymentSchedule);
    }

    public static void MockHouseholdList(this IHouseholdServiceClient householdServiceClient, ICustomerOnSAServiceClient customerOnSAServiceClient)
    {
        var fixture = FixtureFactory.Create();

        var householdMain = fixture.Build<Household>().With(h => h.HouseholdId, 1).With(h => h.HouseholdTypeId, (int)HouseholdTypes.Main).Create();
        var householdCodebtor = fixture.Build<Household>().With(h => h.HouseholdId, 2).With(h => h.HouseholdTypeId, (int)HouseholdTypes.Codebtor).Create();

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
            customer.CustomerIdentifiers.Add(new Identity(0, IdentitySchemes.Kb));

            return customer;
        }
    }


}