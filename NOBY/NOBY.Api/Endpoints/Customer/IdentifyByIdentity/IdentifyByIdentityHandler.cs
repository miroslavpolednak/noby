using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CustomerService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using CIS.Foms.Enums;
using DomainServices.HouseholdService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

internal sealed class IdentifyByIdentityHandler
    : IRequestHandler<IdentifyByIdentityRequest>
{
    public async Task Handle(IdentifyByIdentityRequest request, CancellationToken cancellationToken)
    {
        // customer On SA
        var customerOnSaInstance = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        if (customerOnSaInstance.CustomerIdentifiers is not null && customerOnSaInstance.CustomerIdentifiers.Any())
            throw new NobyValidationException("CustomerOnSA has been already identified");

        var (customersInSA, household, saInstance) = await fetchEntities(customerOnSaInstance.SalesArrangementId, request.CustomerOnSAId, cancellationToken);

        // detail vsech ostatnich customeru na SA
        List<_HO.CustomerOnSA> customerDetails = new();
        foreach (var customer in customersInSA.Where(t => t.CustomerOnSAId != customerOnSaInstance.CustomerOnSAId))
        {
            customerDetails.Add(await _customerOnSAService.GetCustomer(customer.CustomerOnSAId, cancellationToken));
        }

        // validate two same identities on household
        if (customerOnSaInstance.CustomerIdentifiers?.Any() ?? false)
        {
            foreach (var customer in customerDetails)
            {
                if (customerOnSaInstance.CustomerIdentifiers.Any(x => customer.CustomerIdentifiers.Any(t => t.IdentityScheme == x.IdentityScheme && t.IdentityId == x.IdentityId)))
                {
                    throw new NobyValidationException("Identity already present on SalesArrangement customers");
                }
            }
        }

        // update customera
        var updateResult = await updateCustomer(customerOnSaInstance, request.CustomerIdentity!, cancellationToken);

        // hlavni klient
        if (customerOnSaInstance.CustomerRoleId == (int)CustomerRoles.Debtor)
        {
            var notification = new Notifications.MainCustomerUpdatedNotification(saInstance.CaseId, saInstance.SalesArrangementId, request.CustomerOnSAId, updateResult.CustomerIdentifiers);
            await _mediator.Publish(notification, cancellationToken);
        }

        // HFICH-5396
        await updateFlowSwitches(household, customerDetails, request.CustomerOnSAId, cancellationToken);
    }

    private async Task updateFlowSwitches(_HO.Household household, List<_HO.CustomerOnSA> customerDetails, int customerOnSAId, CancellationToken cancellationToken)
    {
        // druhy klient v domacnosti
        var secondCustomerOnHouseholdId = household.CustomerOnSAId1 == customerOnSAId ? household.CustomerOnSAId2 : household.CustomerOnSAId1;
        if (!secondCustomerOnHouseholdId.HasValue || isIdentified())
        {
            var flowSwitchId = household.HouseholdTypeId switch
            {
                (int)HouseholdTypes.Main => FlowSwitches.CustomerIdentifiedOnMainHousehold,
                (int)HouseholdTypes.Codebtor => FlowSwitches.CustomerIdentifiedOnCodebtorHousehold,
                _ => throw new NobyValidationException("Unsupported HouseholdType")
            };

            await _salesArrangementService.SetFlowSwitches(household.SalesArrangementId, new()
            {
                new()
                {
                    FlowSwitchId = (int)flowSwitchId,
                    Value = true
                }
            }, cancellationToken);
        }

        bool isIdentified()
        {
            return customerDetails
                .First(t => t.CustomerOnSAId == secondCustomerOnHouseholdId)
                .CustomerIdentifiers?
                .Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb && t.IdentityId > 0)
                ?? false;
        }
    }

    private async Task<(List<_HO.CustomerOnSA> customersInSA, _HO.Household household, _SA.SalesArrangement saInstance)> fetchEntities(int salesArrangementId, int customerOnSAId, CancellationToken cancellationToken)
    {
        // vsichni customeri na SA
        var customersInSA = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);
        // vsechny households na SA
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        // SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);

        // domacnost aktualniho klienta
        var currentHousehold = households.First(t => t.CustomerOnSAId1 == customerOnSAId || t.CustomerOnSAId2 == customerOnSAId);

        return (customersInSA, currentHousehold, saInstance);
    }

    private async Task<_HO.UpdateCustomerResponse> updateCustomer(_HO.CustomerOnSA customerOnSaInstance, CIS.Foms.Types.CustomerIdentity customerIdentity, CancellationToken cancellationToken)
    {
        // crm customer
        var customerInstance = await _customerService.GetCustomerDetail(customerIdentity, cancellationToken);

        var modelToUpdate = new _HO.UpdateCustomerRequest
        {
            CustomerOnSAId = customerOnSaInstance.CustomerOnSAId,
            Customer = new _HO.CustomerOnSABase
            {
                DateOfBirthNaturalPerson = customerInstance.NaturalPerson.DateOfBirth,
                FirstNameNaturalPerson = customerInstance.NaturalPerson.FirstName,
                Name = customerInstance.NaturalPerson.LastName,
                LockedIncomeDateTime = customerOnSaInstance.LockedIncomeDateTime,
                MaritalStatusId = customerInstance.NaturalPerson?.MaritalStatusStateId
            }
        };
        modelToUpdate.Customer.CustomerIdentifiers.Add(customerIdentity);

        return await _customerOnSAService.UpdateCustomer(modelToUpdate, cancellationToken);
    }

    private readonly IHouseholdServiceClient _householdService;
    private readonly IMediator _mediator;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public IdentifyByIdentityHandler(
        IMediator mediator,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerServiceClient customerService,
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService)
    {
        _householdService = householdService;
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
    }
}
