using DomainServices.SalesArrangementService.Clients;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.CustomerService.Clients;
using _HO = DomainServices.HouseholdService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Customer.IdentifyByIdentity;

// musi tady byt Mediatr.Unit, jinak nefunguje rollback behavior
internal sealed class IdentifyByIdentityHandler(
    IRollbackBag _bag,
    Services.CreateProductTrain.ICreateProductTrainService _createProductTrain,
    Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService _createOrUpdateCustomerKonsDb,
    ISalesArrangementServiceClient _salesArrangementService,
    ICustomerServiceClient _customerService,
    ICustomerOnSAServiceClient _customerOnSAService,
    IHouseholdServiceClient _householdService,
    IProductServiceClient _productService)
        : IRequestHandler<CustomerIdentifyByIdentityRequest, Unit>
{
    public async Task<Unit> Handle(CustomerIdentifyByIdentityRequest request, CancellationToken cancellationToken)
    {
        // customer On SA
        var customerOnSaInstance = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);

        if (customerOnSaInstance.CustomerIdentifiers is not null && customerOnSaInstance.CustomerIdentifiers.Count != 0)
        {
            throw new NobyValidationException("CustomerOnSA has been already identified");
        }

        var (customersInSA, household, saInstance) = await fetchEntities(customerOnSaInstance.SalesArrangementId, request.CustomerOnSAId, cancellationToken);

        // detail vsech ostatnich customeru na SA
        List<_HO.CustomerOnSA> customerDetails = new();
        foreach (var customer in customersInSA.Where(t => t.CustomerOnSAId != customerOnSaInstance.CustomerOnSAId))
        {
            customerDetails.Add(await _customerOnSAService.GetCustomer(customer.CustomerOnSAId, cancellationToken));
        }

        //Debtor has to be identified first
        if (saInstance.IsProductSalesArrangement()
            && customerOnSaInstance.CustomerRoleId is not (int)EnumCustomerRoles.Debtor 
            && !customerDetails.Any(c => c.CustomerRoleId == (int)EnumCustomerRoles.Debtor && c.CustomerIdentifiers.HasKbIdentity()))
        {
            throw new NobyValidationException("Main customer has to be identified first.");
        }

        // validate two same identities on household
        if (customerDetails.Any(x => x.CustomerIdentifiers?.Any(t => (int)t.IdentityScheme == (int)request.CustomerIdentity!.Scheme && t.IdentityId == request.CustomerIdentity.Id) ?? false))
        {
            throw new NobyValidationException(90005);
        }

        // update customera
        var updateResult = await updateCustomer(customerOnSaInstance, request.CustomerIdentity!, cancellationToken);
        _bag.Add(IdentifyByIdentityRollback.BagKeyCustomerOnSA, customerOnSaInstance);

        // pouze pro produktovy SA
        if (saInstance.IsProductSalesArrangement())
        {
            // hlavni klient
            if (customerOnSaInstance.CustomerRoleId == (int)EnumCustomerRoles.Debtor)
            {
                await _createProductTrain.RunAll(saInstance.CaseId, saInstance.SalesArrangementId, request.CustomerOnSAId, updateResult.CustomerIdentifiers, cancellationToken);
            }
            else // vytvoreni klienta v konsDb. Pro dluznika se to dela v notification, pro ostatni se to dubluje tady
            {
                await _createOrUpdateCustomerKonsDb.CreateOrUpdate(updateResult.CustomerIdentifiers, cancellationToken);

                try
                {
                    var partnerId = updateResult.CustomerIdentifiers.GetMpIdentity().IdentityId;
                    var relationshipTypeId = customerOnSaInstance.CustomerRoleId == (int)EnumCustomerRoles.Codebtor ? 2 : 0;

                    await _productService.CreateContractRelationship(partnerId, saInstance.CaseId, relationshipTypeId, cancellationToken);
                }
                catch (CisAlreadyExistsException)
                {
                }
            }

            // HFICH-5396
            await updateFlowSwitches(household, customerDetails, request.CustomerOnSAId, cancellationToken);
        }

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

            await _salesArrangementService.SetFlowSwitch(household.SalesArrangementId, flowSwitchId, true, cancellationToken);
        }

        bool isIdentified()
        {
            return customerDetails
                .First(t => t.CustomerOnSAId == secondCustomerOnHouseholdId)
                .CustomerIdentifiers?
                .HasKbIdentity()
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

    private async Task<_HO.UpdateCustomerResponse> updateCustomer(_HO.CustomerOnSA customerOnSaInstance, SharedTypesCustomerIdentity customerIdentity, CancellationToken cancellationToken)
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
}
