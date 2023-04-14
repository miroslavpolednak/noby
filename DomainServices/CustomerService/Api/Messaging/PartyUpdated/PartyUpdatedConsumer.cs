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
    private readonly ICodebookServiceClients _codebookClient;
    private readonly ICustomerOnSAServiceClient _customerOnSaClient;
    private readonly ISalesArrangementServiceClient _salesArrangementClient;
    private readonly ICaseServiceClient _caseClient;
    public PartyUpdatedConsumer(
        ICodebookServiceClients codebookClient,
        ICustomerOnSAServiceClient customerOnSaClient,
        ISalesArrangementServiceClient salesArrangementClient,
        ICaseServiceClient caseClient)
    {
        _codebookClient = codebookClient;
        _customerOnSaClient = customerOnSaClient;
        _salesArrangementClient = salesArrangementClient;
        _caseClient = caseClient;
    }
    
    public async Task Consume(ConsumeContext<PartyUpdatedV1> context)
    {
        var message = context.Message;
        var party = message.NewParty;

        var maritalStatuses = await _codebookClient.MaritalStatuses(context.CancellationToken);
        var maritalStatusId = maritalStatuses.FirstOrDefault(m => m.RdmMaritalStatusCode == party.NaturalPersonAttributes.MaritalStatusCode)?.Id;
        
        var identity = new Identity
        {
            IdentityId = message.CustomerId,
            IdentityScheme = Identity.Types.IdentitySchemes.Kb
        };
        
        var customerOnSas = await _customerOnSaClient.GetCustomersByIdentity(identity, context.CancellationToken);

        foreach (var customerOnSa in customerOnSas)
        {
            // Update Case
            if (customerOnSa.CustomerRoleId == 1)
            {
                var salesArrangement = await _salesArrangementClient.GetSalesArrangement(customerOnSa.SalesArrangementId, context.CancellationToken);
                var @case = await _caseClient.GetCaseDetail(salesArrangement.CaseId, context.CancellationToken);
                var customerData = new CustomerData
                {
                    Cin = @case.Customer.Cin,
                    Identity = @case.Customer.Identity,
                    FirstNameNaturalPerson = party.NaturalPersonAttributes.FirstName,
                    Name = party.NaturalPersonAttributes.Surname,
                    DateOfBirthNaturalPerson = party.NaturalPersonAttributes.BirthDate.Date
                };
                await _caseClient.UpdateCustomerData(salesArrangement.CaseId, customerData, context.CancellationToken);
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
            
            await _customerOnSaClient.UpdateCustomer(updateCustomer, context.CancellationToken);
        }
    }
}