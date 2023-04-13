using DomainServices.CaseService.Clients;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using MassTransit;
using UpdateCustomerRequest = DomainServices.HouseholdService.Contracts.UpdateCustomerRequest;

namespace DomainServices.CustomerService.Api.Messaging.PartyUpdated;

public class PartyUpdatedConsumer : IConsumer<PartyUpdatedV1>
{
    private readonly IServiceProvider _serviceProvider;
    public PartyUpdatedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task Consume(ConsumeContext<PartyUpdatedV1> context)
    {
        var message = context.Message;
        var party = message.NewParty;
        
        using var scope = _serviceProvider.CreateScope();
        var codebookClient = scope.ServiceProvider.GetRequiredService<ICodebookServiceClients>();
        var customerOnSaClient = scope.ServiceProvider.GetRequiredService<ICustomerOnSAServiceClient>();
        var salesArrangementClient = scope.ServiceProvider.GetRequiredService<ISalesArrangementServiceClient>();
        var caseClient = scope.ServiceProvider.GetRequiredService<ICaseServiceClient>();

        var maritalStatuses = await codebookClient.MaritalStatuses(context.CancellationToken);
        var maritalStatusId = maritalStatuses.FirstOrDefault(m => m.RdmMaritalStatusCode == party.NaturalPersonAttributes.MaritalStatusCode)?.Id;
        
        var identity = new Identity
        {
            IdentityId = message.CustomerId,
            IdentityScheme = Identity.Types.IdentitySchemes.Kb
        };
        
        var customerOnSas = await customerOnSaClient.GetCustomersByIdentity(identity, context.CancellationToken);

        foreach (var customerOnSa in customerOnSas)
        {
            // Update Case
            if (customerOnSa.CustomerRoleId == 1)
            {
                var salesArrangement = await salesArrangementClient.GetSalesArrangement(customerOnSa.SalesArrangementId, context.CancellationToken);
                var @case = await caseClient.GetCaseDetail(salesArrangement.CaseId, context.CancellationToken);
                var customerData = new CustomerData
                {
                    Cin = @case.Customer.Cin,
                    Identity = @case.Customer.Identity,
                    FirstNameNaturalPerson = party.NaturalPersonAttributes.FirstName,
                    Name = party.NaturalPersonAttributes.Surname,
                    DateOfBirthNaturalPerson = party.NaturalPersonAttributes.BirthDate.Date
                };
                await caseClient.UpdateCustomerData(salesArrangement.CaseId, customerData, context.CancellationToken);
            }
            
            // Update CustomerOnSA
            var updateCustomer = new UpdateCustomerRequest
            {
                CustomerOnSAId = customerOnSa.CustomerOnSAId,
                Customer = new CustomerOnSABase
                {
                    FirstNameNaturalPerson = party.NaturalPersonAttributes.FirstName,
                    Name = party.NaturalPersonAttributes.Surname,
                    DateOfBirthNaturalPerson = party.NaturalPersonAttributes.BirthDate.Date,
                    MaritalStatusId = maritalStatusId
                }
            };
            
            await customerOnSaClient.UpdateCustomer(updateCustomer, context.CancellationToken);
        }
        
    }
}