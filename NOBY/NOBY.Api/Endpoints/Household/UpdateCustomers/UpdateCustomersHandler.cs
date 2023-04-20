#region usings
using CIS.Foms.Enums;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using __HO = DomainServices.HouseholdService.Contracts;
using DomainServices.CustomerService.Clients;
#endregion usings
namespace NOBY.Api.Endpoints.Household.UpdateCustomers;

internal sealed class UpdateCustomersHandler
    : IRequestHandler<UpdateCustomersRequest, UpdateCustomersResponse>
{
    public async Task<UpdateCustomersResponse> Handle(UpdateCustomersRequest request, CancellationToken cancellationToken)
    {
        // detail domacnosti - kontrola existence (404)
        var householdInstance = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);
        // potrebuju i caseId
        var salesArrangement = await _salesArrangementService.GetSalesArrangement(householdInstance.SalesArrangementId, cancellationToken);

        // zkontrolovat, zda neni customer jiz v jine domacnosti
        var allCustomers = await checkDoubledCustomers(householdInstance.SalesArrangementId, request, cancellationToken);

        // process customer1
        var c1 = await crudCustomer(request.Customer1, salesArrangement.SalesArrangementId, salesArrangement.CaseId, householdInstance.CustomerOnSAId1, CustomerRoles.Debtor, allCustomers, cancellationToken);
    
        // process customer2
        var c2 = await crudCustomer(request.Customer2, salesArrangement.SalesArrangementId, salesArrangement.CaseId, householdInstance.CustomerOnSAId2, CustomerRoles.Codebtor, allCustomers, cancellationToken);

        // linkovani novych nebo zmenenych CustomerOnSAId na household
        if (householdInstance.CustomerOnSAId1 != c1.OnHouseholdCustomerOnSAId || householdInstance.CustomerOnSAId2 != c2.OnHouseholdCustomerOnSAId) 
        {
            await _householdService.LinkCustomerOnSAToHousehold(householdInstance.HouseholdId, c1.OnHouseholdCustomerOnSAId, c2.OnHouseholdCustomerOnSAId, cancellationToken);
        }

        // zastavit podepisovani, pokud probehla zmena na customerech
        if (c1.CancelSigning || c2.CancelSigning)
        {
            var documentsToSign = await _documentOnSAService.GetDocumentsToSignList(householdInstance.SalesArrangementId, cancellationToken);
            foreach (var document in documentsToSign.DocumentsOnSAToSign.Where(t => t.DocumentOnSAId.HasValue && t.IsValid))
            {
                await _documentOnSAService.StopSigning(document.DocumentOnSAId!.Value, cancellationToken);
            }

            // HFICH-4165 - nastaveni flowSwitches
            bool isSecondCustomerIdentified = c2.Identities?.Any(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb) ?? false;
            await setFlowSwitches(householdInstance.HouseholdTypeId, householdInstance.SalesArrangementId, isSecondCustomerIdentified, cancellationToken);
        }

        return new UpdateCustomersResponse
        {
            CustomerOnSAId1 = c1.OnHouseholdCustomerOnSAId,
            CustomerOnSAId2 = c2.OnHouseholdCustomerOnSAId
        };
    }

    /// <summary>
    /// Nastavit flowSwitches na SalesArrangementu
    /// </summary>
    private async Task setFlowSwitches(int householdTypeId, int salesArrangementId, bool isSecondCustomerIdentified, CancellationToken cancellationToken)
    {
        // kolekce flow switches, kterou na konci ulozime na SA
        var flowSwitchesToSet = new List<DomainServices.SalesArrangementService.Contracts.FlowSwitch>();

        switch (householdTypeId)
        {
            case (int)HouseholdTypes.Main:
                flowSwitchesToSet.Add(new()
                {
                    FlowSwitchId = (int)FlowSwitches.Was3601MainChangedAfterSigning,
                    Value = true
                });

                // HFICH-5396, vezmi druhého customera z householdu a pokud existuje, tak zkontroluj, že pro identity.scheme = KBID existuje identity.id které není null
                flowSwitchesToSet.Add(new()
                {
                    FlowSwitchId = (int)FlowSwitches.CustomerIdentifiedOnMainHousehold,
                    Value = isSecondCustomerIdentified
                });
                break;
            case (int)HouseholdTypes.Codebtor:
                flowSwitchesToSet.Add(new()
                {
                    FlowSwitchId = (int)FlowSwitches.Was3602CodebtorChangedAfterSigning,
                    Value = true
                });

                // HFICH-5396, vezmi druhého customera z householdu a pokud existuje, tak zkontroluj, že pro identity.scheme = KBID existuje identity.id které není null
                flowSwitchesToSet.Add(new()
                {
                    FlowSwitchId = (int)FlowSwitches.CustomerIdentifiedOnCodebtorHousehold,
                    Value = isSecondCustomerIdentified
                });
                break;
            default:
                throw new NobyValidationException("Unsupported HouseholdType");
        }

        // pokud jsou nejake flow switches k nastaveni
        if (flowSwitchesToSet.Any())
        {
            await _salesArrangementService.SetFlowSwitches(salesArrangementId, flowSwitchesToSet, cancellationToken);
        }
    }

    /// <summary>
    /// Kontrola zda nektery z customeru jiz nema pouzitou stejnou identitu
    /// </summary>
    private async Task<List<__HO.CustomerOnSA>> checkDoubledCustomers(int salesArrangementId, UpdateCustomersRequest request, CancellationToken cancellationToken)
    {
        var allHouseholds = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        var allCustomers = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);
        
        var customers = allHouseholds
            .Where(t => t.HouseholdId != request.HouseholdId && t.CustomerOnSAId1.HasValue)
            .Select(t => new { CustomerOnSAId = t.CustomerOnSAId1!.Value, KbId = allCustomers.First(x => x.CustomerOnSAId == t.CustomerOnSAId1).CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId })
            .Union(allHouseholds
                .Where(t => t.HouseholdId != request.HouseholdId && t.CustomerOnSAId2.HasValue)
                .Select(t => new { CustomerOnSAId = t.CustomerOnSAId2!.Value, KbId = allCustomers.First(x => x.CustomerOnSAId == t.CustomerOnSAId2).CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId })
            );

        if (customers.Any(t => t.CustomerOnSAId == request.Customer1?.CustomerOnSAId || t.CustomerOnSAId == request.Customer2?.CustomerOnSAId))
            throw new NobyValidationException(90005, "The same CustomerOnSAId already exist in another household");

        if (customers.Any(t => t.CustomerOnSAId == request.Customer1?.Identity?.Id || t.CustomerOnSAId == request.Customer2?.Identity?.Id))
            throw new NobyValidationException(90005, "The same KBID already exist in another household");

        return allCustomers;
    }

    private async Task<Dto.CrudResult> crudCustomer(
        Dto.CustomerDto? customer,
        int salesArrangementId,
        long caseId,
        int? customerOnSAId,
        CustomerRoles customerRole,
        List<__HO.CustomerOnSA> allCustomers,
        CancellationToken cancellationToken)
    {
        // smazat existujiciho, neni misto nej zadny novy
        if (customer is null && customerOnSAId.HasValue)
        {
            await _customerOnSAService.DeleteCustomer(customerOnSAId.Value, cancellationToken: cancellationToken);
            await deleteRelationships(caseId, getMpId(customerOnSAId.Value), cancellationToken);

            return new Dto.CrudResult(true);
        }
        else if (customer is not null)
        {
            bool customerIdChanged = customer.CustomerOnSAId != customerOnSAId && customerOnSAId.HasValue;

            // smazat existujiciho, je nahrazen novym
            if (customerIdChanged)
            {
                await _customerOnSAService.DeleteCustomer(customerOnSAId!.Value, cancellationToken: cancellationToken);
                await deleteRelationships(caseId, getMpId(customerOnSAId!.Value), cancellationToken);
            }

            // update stavajiciho
            if (customer.CustomerOnSAId.HasValue)
            {
                var result = new Dto.CrudResult(customerIdChanged, customer.CustomerOnSAId.Value);
                
                try
                {
                    var currentCustomerInstance = allCustomers.First(t => t.CustomerOnSAId == customer.CustomerOnSAId!.Value);

                    var identities = (await _customerOnSAService.UpdateCustomer(new __HO.UpdateCustomerRequest
                        {
                            CustomerOnSAId = customer.CustomerOnSAId!.Value,
                            Customer = customer.ToDomainServiceRequest(currentCustomerInstance.LockedIncomeDateTime)
                        }, cancellationToken))
                        .CustomerIdentifiers;

                    result.Identities = identities;
                }
                catch (CisArgumentException ex) when (ex.ExceptionCode == "16033")
                {
                    // osetrena vyjimka, kdy je klient identifikovan KB identitou, ale nepodarilo se vytvorit identitu v MP
                    //TODO je otazka, jak se zde zachovat?
                }

                return result;
            }
            else // vytvoreni noveho
            {
                var createResult = await _customerOnSAService.CreateCustomer(new __HO.CreateCustomerRequest
                {
                    SalesArrangementId = salesArrangementId,
                    CustomerRoleId = (int)customerRole,
                    Customer = customer.ToDomainServiceRequest()
                }, cancellationToken);

                return new Dto.CrudResult(true, createResult.CustomerOnSAId)
                {
                    Identities = createResult.CustomerIdentifiers
                };
            }
        }
        else
        {
            return new Dto.CrudResult();
        }

        long? getMpId(int customerOnSAId)
        {
            return allCustomers
                .First(t => t.CustomerOnSAId == customerOnSAId)
                .CustomerIdentifiers?
                .FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)?
                .IdentityId;
        }
    }

    /// <summary>
    /// Upravit relationship v KonsDb
    /// </summary>
    private async Task deleteRelationships(long caseId, long? partnerId, CancellationToken cancellationToken)
    {
        if (!partnerId.HasValue) return;

        try
        {
            await _productService.DeleteContractRelationship(partnerId.Value, caseId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Can not delete relationship for #{partnerId} in Case {caseId}: {ex.Message}");
        }
    }

    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly IProductServiceClient _productService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ILogger<UpdateCustomersHandler> _logger;

    public UpdateCustomersHandler(
        ILogger<UpdateCustomersHandler> logger,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSAService,
        IProductServiceClient productService,
        IDocumentOnSAServiceClient documentOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerServiceClient customerService)
    {
        _logger = logger;
        _customerService = customerService;
        _productService = productService;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
        _documentOnSAService = documentOnSAService;
        _salesArrangementService = salesArrangementService;
    }
}
