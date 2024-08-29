using cz.kb.cm.be.@event.partyupdated.v1;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using KafkaFlow;
using SharedTypes.GrpcTypes;
using UpdateCustomerRequest = DomainServices.HouseholdService.Contracts.UpdateCustomerRequest;

namespace DomainServices.CustomerService.Api.Messaging.PartyUpdated;

internal sealed class PartyUpdatedHandler : IMessageHandler<PartyUpdatedV1>
{
    private readonly ICodebookServiceClient _codebookClient;
    private readonly ICustomerOnSAServiceClient _customerOnSaClient;
    private readonly ISalesArrangementServiceClient _salesArrangementClient;
    private readonly ICaseServiceClient _caseClient;

    public PartyUpdatedHandler(
        ICodebookServiceClient codebookClient,
        ICustomerOnSAServiceClient customerOnSaClient,
        ISalesArrangementServiceClient salesArrangementClient,
        ICaseServiceClient caseClient)
    {
        _codebookClient = codebookClient;
        _customerOnSaClient = customerOnSaClient;
        _salesArrangementClient = salesArrangementClient;
        _caseClient = caseClient;
    }

    public async Task Handle(IMessageContext context, PartyUpdatedV1 message)
    {
        var party = message.NewParty;

        var maritalStatuses = await _codebookClient.MaritalStatuses();
        var maritalStatusId = maritalStatuses.FirstOrDefault(m => m.RdmCode == party.NaturalPersonAttributes?.MaritalStatusCode)?.Id;

        var identity = new Identity
        {
            IdentityId = message.CustomerId,
            IdentityScheme = Identity.Types.IdentitySchemes.Kb
        };

        var customerOnSas = await _customerOnSaClient.GetCustomersByIdentity(identity);
        var cases = await _caseClient.GetCasesByIdentity(identity);
        var casesDict = cases.ToDictionary(c => c.CaseId);

        foreach (var customerOnSa in customerOnSas)
        {
            // ziskat rozdilovou deltu pokud existuje
            var delta = customerOnSa.GetCustomerChangeDataObject();

            // Update Case
            if (customerOnSa.CustomerRoleId == 1)
            {
                var salesArrangement = await _salesArrangementClient.GetSalesArrangement(customerOnSa.SalesArrangementId);

                if (casesDict.TryGetValue(salesArrangement.CaseId, out var @case))
                {
                    cases.Remove(@case);
                }
                else
                {
                    @case = await _caseClient.GetCaseDetail(salesArrangement.CaseId);
                }

                var customerData = new CustomerData
                {
                    Cin = @case.Customer.Cin,
                    Identity = @case.Customer.Identity,
                    FirstNameNaturalPerson = OriginalOrDelta(delta?.NaturalPerson?.FirstName, party.NaturalPersonAttributes.FirstName),
                    Name = OriginalOrDelta(delta?.NaturalPerson?.LastName, party.NaturalPersonAttributes.Surname),
                    DateOfBirthNaturalPerson = OriginalOrDeltaD(delta?.NaturalPerson?.DateOfBirth, party.NaturalPersonAttributes.BirthDate.Date)
                };
                await _caseClient.UpdateCustomerData(salesArrangement.CaseId, customerData);
            }

            // Update CustomerOnSA
            var updateCustomer = new UpdateCustomerRequest
            {
                CustomerOnSAId = customerOnSa.CustomerOnSAId,
                Customer = new CustomerOnSABase
                {
                    FirstNameNaturalPerson = OriginalOrDelta(delta?.NaturalPerson?.FirstName, party.NaturalPersonAttributes.FirstName),
                    Name = OriginalOrDelta(delta?.NaturalPerson?.LastName, party.NaturalPersonAttributes.Surname),
                    DateOfBirthNaturalPerson = OriginalOrDeltaD(delta?.NaturalPerson?.DateOfBirth, party.NaturalPersonAttributes.BirthDate.Date),
                    MaritalStatusId = OriginalOrDeltaI(delta?.NaturalPerson?.MaritalStatusId, maritalStatusId)
                }
            };

            await _customerOnSaClient.UpdateCustomer(updateCustomer);
        }

        foreach (var @case in cases)
        {
            var customerData = new CustomerData
            {
                Cin = @case.Customer.Cin,
                Identity = @case.Customer.Identity,
                FirstNameNaturalPerson = party.NaturalPersonAttributes.FirstName,
                Name = party.NaturalPersonAttributes.Surname,
                DateOfBirthNaturalPerson = party.NaturalPersonAttributes.BirthDate.Date
            };
            await _caseClient.UpdateCustomerData(@case.CaseId, customerData);
        }
    }

    private static string? OriginalOrDelta(string? delta, string? original)
        => string.IsNullOrEmpty(delta) ? original : delta;

    private static DateTime? OriginalOrDeltaD(DateTime? delta, DateTime? original)
        => delta ?? original;

    private static int? OriginalOrDeltaI(int? delta, int? original)
        => delta ?? original;
}