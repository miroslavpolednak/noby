using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.CustomerService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using CIS.Foms.Enums;
using CIS.Infrastructure.CisMediatR.Rollback;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

// musi tady byt Mediatr.Unit, jinak nefunguje rollback behavior
internal sealed class IdentifyByIdentityHandler
    : IRequestHandler<IdentifyByIdentityRequest, MediatR.Unit>
{
    public async Task<Unit> Handle(IdentifyByIdentityRequest request, CancellationToken cancellationToken)
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
        if (customerDetails.Any(x => x.CustomerIdentifiers?.Any(t => (int)t.IdentityScheme == (int)request.CustomerIdentity!.Scheme && t.IdentityId == request.CustomerIdentity.Id) ?? false))
        {
            throw new NobyValidationException("Identity already present on SalesArrangement customers");
        }

        // update customera
        var updateResult = await updateCustomer(customerOnSaInstance, request.CustomerIdentity!, cancellationToken);
        _bag.Add(IdentifyByIdentityRollback.BagKeyCustomerOnSA, customerOnSaInstance);

        // hlavni klient
        if (customerOnSaInstance.CustomerRoleId == (int)CustomerRoles.Debtor)
        {
            var notification = new Notifications.MainCustomerUpdatedNotification(saInstance.CaseId, saInstance.SalesArrangementId, request.CustomerOnSAId, updateResult.CustomerIdentifiers);
            await _mediator.Publish(notification, cancellationToken);

            await _salesArrangementService.SetContractNumber(saInstance.SalesArrangementId, request.CustomerOnSAId, cancellationToken);
        }
        else // vytvoreni klienta v konsDb. Pro dluznika se to dela v notification, pro ostatni se to dubluje tady
        {
            await _createOrUpdateCustomerKonsDb.CreateOrUpdate(updateResult.CustomerIdentifiers, cancellationToken);

            var partnerId = updateResult.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Mp).IdentityId;
            var relationshipTypeId = customerOnSaInstance.CustomerRoleId == (int)CustomerRoles.Codebtor ? 2 : 0;

            await _productService.CreateContractRelationship(partnerId, saInstance.CaseId, relationshipTypeId, cancellationToken);
        }

        // HFICH-5396
        await updateFlowSwitches(household, customerDetails, request.CustomerOnSAId, cancellationToken);

        return new MediatR.Unit();
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

    private readonly IRollbackBag _bag;
    private readonly Infrastructure.Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService _createOrUpdateCustomerKonsDb;
    private readonly IHouseholdServiceClient _householdService;
    private readonly IProductServiceClient _productService;
    private readonly IMediator _mediator;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public IdentifyByIdentityHandler(
        IRollbackBag bag,
        IMediator mediator,
        Infrastructure.Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService createOrUpdateCustomerKonsDb,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerServiceClient customerService,
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService, 
        IProductServiceClient productService)
    {
        _bag = bag;
        _createOrUpdateCustomerKonsDb = createOrUpdateCustomerKonsDb;
        _householdService = householdService;
        _productService = productService;
        _mediator = mediator;
        _salesArrangementService = salesArrangementService;
        _customerService = customerService;
        _customerOnSAService = customerOnSAService;
    }
}
