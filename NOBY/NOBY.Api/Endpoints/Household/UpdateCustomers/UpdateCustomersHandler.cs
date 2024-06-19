using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using __HO = DomainServices.HouseholdService.Contracts;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Services.FlowSwitchAtLeastOneIncomeMainHousehold;
using NOBY.Services.SigningHelper;

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
        bool isProductSA = salesArrangement.IsProductSalesArrangement();

        // prevent changin main debtor
        if (householdInstance.HouseholdTypeId == (int)HouseholdTypes.Main
            && householdInstance.CustomerOnSAId1 != request.Customer1?.CustomerOnSAId)
        {
            throw new NobyValidationException("prevent changin main debtor");
        }

        // zkontrolovat, zda neni customer jiz v jine domacnosti
        var allCustomers = await checkDoubledCustomers(householdInstance.SalesArrangementId, request, cancellationToken);

        // process customer1
        var c1 = await crudCustomer(request.Customer1, salesArrangement.SalesArrangementId, isProductSA, salesArrangement.CaseId, householdInstance.CustomerOnSAId1, CustomerRoles.Debtor, allCustomers, cancellationToken);

        // process customer2
        var c2 = await crudCustomer(request.Customer2, salesArrangement.SalesArrangementId, isProductSA, salesArrangement.CaseId, householdInstance.CustomerOnSAId2, CustomerRoles.Codebtor, allCustomers, cancellationToken);

        // linkovani novych nebo zmenenych CustomerOnSAId na household
        if (householdInstance.CustomerOnSAId1 != c1.OnHouseholdCustomerOnSAId || householdInstance.CustomerOnSAId2 != c2.OnHouseholdCustomerOnSAId)
        {
            await _householdService.LinkCustomerOnSAToHousehold(householdInstance.HouseholdId, c1.OnHouseholdCustomerOnSAId, c2.OnHouseholdCustomerOnSAId, cancellationToken);
        }

        // zastavit podepisovani, pokud probehla zmena na customerech
        if (c1.Reason != Dto.CrudResult.Reasons.None || c2.Reason != Dto.CrudResult.Reasons.None)
        {
            var documentsToSign = await _documentOnSAService.GetDocumentsToSignList(salesArrangement.SalesArrangementId, cancellationToken);
            bool onlyNotSigned = (c1.Reason == Dto.CrudResult.Reasons.CustomerRemoved && c2.Reason == Dto.CrudResult.Reasons.None)
                || (c1.Reason == Dto.CrudResult.Reasons.None && c2.Reason == Dto.CrudResult.Reasons.CustomerRemoved)
                || (c1.Reason == Dto.CrudResult.Reasons.CustomerRemoved && c2.Reason == Dto.CrudResult.Reasons.CustomerRemoved);

            foreach (var document in documentsToSign.DocumentsOnSAToSign.Where(t => t.DocumentOnSAId.HasValue && t.HouseholdId == request.HouseholdId && (!t.IsSigned || !onlyNotSigned)))
            {
                await _signingHelperService.StopSinningAccordingState(new()
                {
                    DocumentOnSAId = document.DocumentOnSAId!.Value,
                    SignatureTypeId = document.SignatureTypeId,
                    SalesArrangementId = document.SalesArrangementId
                }, cancellationToken);
            }

            // HFICH-4165
            if (!onlyNotSigned)
            {
                _flowSwitchManager.AddFlowSwitch(householdInstance.HouseholdTypeId == (int)HouseholdTypes.Main ? FlowSwitches.Was3601MainChangedAfterSigning : FlowSwitches.Was3602CodebtorChangedAfterSigning, true);
            }

            // HFICH-4165 - nastaveni flowSwitches
            bool isFirstCustomerIdentified = !c1.OnHouseholdCustomerOnSAId.HasValue || (c1.Identities?.Any(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb) ?? false);
            bool isSecondCustomerIdentified = !c2.OnHouseholdCustomerOnSAId.HasValue || (c2.Identities?.Any(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb) ?? false);
            _flowSwitchManager.AddFlowSwitch(householdInstance.HouseholdTypeId == (int)HouseholdTypes.Main ? FlowSwitches.CustomerIdentifiedOnMainHousehold : FlowSwitches.CustomerIdentifiedOnCodebtorHousehold, isFirstCustomerIdentified && isSecondCustomerIdentified);
        }

        _flowSwitchManager.AddFlowSwitch(FlowSwitches.ScoringPerformedAtleastOnce, false);

        if (isProductSA)
        {
            await _flowSwitchMainHouseholdService.SetFlowSwitchByHouseholdId(request.HouseholdId, _flowSwitchManager, cancellationToken);
        }

        await _flowSwitchManager.SaveFlowSwitches(householdInstance.SalesArrangementId, cancellationToken);

        return new UpdateCustomersResponse
        {
            CustomerOnSAId1 = c1.OnHouseholdCustomerOnSAId,
            CustomerOnSAId2 = c2.OnHouseholdCustomerOnSAId
        };
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
            .Select(t => new { CustomerOnSAId = t.CustomerOnSAId1!.Value, KbId = allCustomers.First(x => x.CustomerOnSAId == t.CustomerOnSAId1).CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId })
            .Union(allHouseholds
                .Where(t => t.HouseholdId != request.HouseholdId && t.CustomerOnSAId2.HasValue)
                .Select(t => new { CustomerOnSAId = t.CustomerOnSAId2!.Value, KbId = allCustomers.First(x => x.CustomerOnSAId == t.CustomerOnSAId2).CustomerIdentifiers?.FirstOrDefault(x => x.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId })
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
        bool isProductSA,
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
            if (isProductSA)
                await deleteRelationships(caseId, getMpId(customerOnSAId.Value), cancellationToken);

            return new Dto.CrudResult(Dto.CrudResult.Reasons.CustomerRemoved);
        }
        else if (customer is not null)
        {
            bool customerIdChanged = customer.CustomerOnSAId != customerOnSAId && customerOnSAId.HasValue;

            // smazat existujiciho, je nahrazen novym
            if (customerIdChanged)
            {
                await _customerOnSAService.DeleteCustomer(customerOnSAId!.Value, cancellationToken: cancellationToken);
                if (isProductSA)
                    await deleteRelationships(caseId, getMpId(customerOnSAId!.Value), cancellationToken);
            }

            // update stavajiciho
            if (customer.CustomerOnSAId.HasValue)
            {
                var result = new Dto.CrudResult(customerIdChanged ? Dto.CrudResult.Reasons.CustomerUpdated : Dto.CrudResult.Reasons.None, customer.CustomerOnSAId.Value);

                try
                {
                    var currentCustomerInstance = allCustomers.First(t => t.CustomerOnSAId == customer.CustomerOnSAId!.Value);

                    var identities = (await _customerOnSAService.UpdateCustomer(new __HO.UpdateCustomerRequest
                    {
                        CustomerOnSAId = customer.CustomerOnSAId!.Value,
                        Customer = customer.ToDomainServiceRequest(currentCustomerInstance)
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

                return new Dto.CrudResult(Dto.CrudResult.Reasons.CustomerAdded, createResult.CustomerOnSAId)
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
                .FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp)?
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
            _logger.LogInformation(ex, "Can not delete relationship for #{PartnerId} in Case {CaseId}: {Message}", partnerId, caseId, ex.Message);
        }
    }

    private readonly FlowSwitchAtLeastOneIncomeMainHouseholdService _flowSwitchMainHouseholdService;
    private readonly IFlowSwitchManager _flowSwitchManager;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ISigningHelperService _signingHelperService;
    private readonly IProductServiceClient _productService;
    private readonly ILogger<UpdateCustomersHandler> _logger;

    public UpdateCustomersHandler(
        FlowSwitchAtLeastOneIncomeMainHouseholdService flowSwitchMainHouseholdService,
        ILogger<UpdateCustomersHandler> logger,
        IFlowSwitchManager flowSwitchManager,
        IHouseholdServiceClient householdService,
        ICustomerOnSAServiceClient customerOnSAService,
        IProductServiceClient productService,
        IDocumentOnSAServiceClient documentOnSAService,
        ISalesArrangementServiceClient salesArrangementService,
        ISigningHelperService signingHelperService)
    {
        _flowSwitchMainHouseholdService = flowSwitchMainHouseholdService;
        _logger = logger;
        _flowSwitchManager = flowSwitchManager;
        _productService = productService;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
        _documentOnSAService = documentOnSAService;
        _salesArrangementService = salesArrangementService;
        _signingHelperService = signingHelperService;
    }
}
