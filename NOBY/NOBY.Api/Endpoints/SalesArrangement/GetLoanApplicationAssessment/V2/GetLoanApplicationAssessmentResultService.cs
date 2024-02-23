using CIS.Core.Attributes;
using CIS.Core.Security;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.HouseholdService.Contracts;
using DomainServices.OfferService.Contracts;
using DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2;
using DomainServices.RiskIntegrationService.Contracts.Shared;
using DomainServices.RiskIntegrationService.Contracts.Shared.V1;
using DomainServices.UserService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment.V2;

[ScopedService, SelfService]
internal sealed class GetLoanApplicationAssessmentResultService
{
    public async Task<GetLoanApplicationAssessmentResponse> CreateResult(
        int salesArrangementId,
        LoanApplicationAssessmentResponse assessment,
        CustomerExposureCalculateResponse? exposure,
        GetOfferResponse offer,
        CancellationToken cancellationToken)
    {
        // customers
        _customers = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);
        // households
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);
        // codebooks
        _obligationTypes = await _codebookService.ObligationTypes(cancellationToken);
        _laExposureItems = await _codebookService.ObligationLaExposures(cancellationToken);

        // get obligations for each customer
        //TODO new DS endpoint for all obligations on SA?
        _obligations = new(_customers.Count);
        foreach (var customer in _customers)
        {
            var list = await _customerOnSAService.GetObligationList(customer.CustomerOnSAId, cancellationToken);
            _obligations[customer.CustomerOnSAId] = list ?? new List<Obligation>(0);
        }

        // vytvoreni response
        GetLoanApplicationAssessmentResponse response = new()
        {
            RiskBusinesscaseExpirationDate = assessment!.RiskBusinessCaseExpirationDate,
            AssessmentResult = assessment.AssessmentResult,
            Application = assessment.ToLoanApplicationApiResponse(offer),
            Reasons = assessment.ToReasonsApiResponse(),
            Households = new(households.Count),
            DisplayAssessmentResultInfoText = assessment.GetDisplayAssessmentResultInfoTextToReasonsApiResponse(_customers, _obligations),
            DisplayWarningExposureDoesNotWork = exposure is null
        };

        // vytvoreni response - households
        foreach (var household in households)
        {
            Dto.Household householdResponse = new()
            {
                HouseholdId = household.HouseholdId,
                HouseholdTypeId = household.HouseholdTypeId,
                Risk = assessment.HouseholdsDetails?.FirstOrDefault(t => t.HouseholdId.GetValueOrDefault() == household.HouseholdId)?.ToHouseholdRiskApiResponse(),
                Customers = new List<Dto.HouseholdCustomerObligations>()
            };

            if (household.CustomerOnSAId1.HasValue)
            {
                householdResponse.Customers.Add(getCustomer(household.CustomerOnSAId1.Value, exposure));
            }

            if (household.CustomerOnSAId2.HasValue)
            {
                householdResponse.Customers.Add(getCustomer(household.CustomerOnSAId2.Value, exposure));
            }

            response.Households.Add(householdResponse);
        }

        return response;
    }

    private Dto.HouseholdCustomerObligations getCustomer(
        int customerOnSAId,
        CustomerExposureCalculateResponse? exposure)
    {
        var obligations = _obligations[customerOnSAId];
        var customer = _customers.First(t => t.CustomerOnSAId == customerOnSAId);
        var customerExposure = exposure?.Customers?.FirstOrDefault(t => t.InternalCustomerId == customer.CustomerOnSAId);

        Dto.HouseholdCustomerObligations obligationCustomer = new()
        {
            DateOfBirth = customer.DateOfBirthNaturalPerson,
            FirstName = customer.FirstNameNaturalPerson,
            LastName = customer.Name,
            RoleId = (CustomerRoles)customer.CustomerRoleId,
            
        };

        if (customerExposure is null)
        {
            obligationCustomer.ExistingObligations = obligations.CreateHouseholdObligations(_obligationTypes)
                .OrderBy(t => t.ObligationTypeOrder)
                .ToList();
        }
        else
        {
            var otherCustomers = exposure!
                .Customers
                ?.Where(t => t.InternalCustomerId.HasValue && t.InternalCustomerId != customerExposure?.InternalCustomerId)
                ?.ToList();

            obligationCustomer.ExistingObligations = getExistingObligations(customerExposure, obligations, otherCustomers);

            if (_currentUser.HasPermission(UserPermissions.CLIENT_EXPOSURE_DisplayRequestedExposure))
            {
                obligationCustomer.RequestedObligations = getRequestedObligations(customerExposure, otherCustomers);
            }
        }
        
        return obligationCustomer;
    }

    private List<Dto.HouseholdObligationItem> getRequestedObligations(
        CustomerExposureCustomer customerExposure,
        List<CustomerExposureCustomer>? otherCustomers)
    {
        var obligations = new List<Dto.HouseholdObligationItem>();

        foreach (var exp in (customerExposure.RequestedCBCBNaturalPersonExposureItem ?? _emptyRequestedCBCBGroupItems))
        {
            var itemToAdd = addRequestedCbCbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false), otherCustomers!, exp.CbcbContractId);
            obligations.Add(itemToAdd);
        }

        foreach (var exp in (customerExposure.RequestedCBCBJuridicalPersonExposureItem ?? _emptyRequestedCBCBGroupItems))
        {
            var itemToAdd = addRequestedCbCbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true), otherCustomers!, exp.CbcbContractId);
            obligations.Add(itemToAdd);
        }

        foreach (var exp in (customerExposure.RequestedKBGroupNaturalPersonExposures ?? _emptyRequestedKBGroupItems))
        {
            var itemToAdd = addRequestedKbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false), otherCustomers!, exp.RiskBusinessCaseId);
            obligations.Add(itemToAdd);
        }

        foreach (var exp in (customerExposure.RequestedKBGroupJuridicalPersonExposures ?? _emptyRequestedKBGroupItems))
        {
            var itemToAdd = addRequestedKbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true), otherCustomers!, exp.RiskBusinessCaseId);
            obligations.Add(itemToAdd);
        }

        return obligations
            .OrderBy(t => t.ObligationTypeOrder)
            .ToList();
    }

    private List<Dto.HouseholdObligationItem> getExistingObligations(
        CustomerExposureCustomer customerExposure, 
        List<Obligation> nobyObligations,
        List<CustomerExposureCustomer>? otherCustomers)
    {
        var obligations = new List<Dto.HouseholdObligationItem>();
        
        foreach (var exp in (customerExposure.ExistingCBCBNaturalPersonExposureItem ?? _emptyExistingCBCBGroupItems))
        {
            var itemToAdd = addExistingCbCbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false), otherCustomers!, exp.CbcbContractId);
            obligations.Add(itemToAdd);
        }

        foreach (var exp in (customerExposure.ExistingCBCBJuridicalPersonExposureItem ?? _emptyExistingCBCBGroupItems))
        {
            var itemToAdd = addExistingCbCbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true), otherCustomers!, exp.CbcbContractId);
            obligations.Add(itemToAdd);
        }
    
        foreach (var exp in (customerExposure.ExistingKBGroupNaturalPersonExposures ?? _emptyExistingKBGroupItems))
        {
            var itemToAdd = addExistingKbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false), otherCustomers!, exp.BankAccount);
            obligations.Add(itemToAdd);
        }

        foreach (var exp in (customerExposure.ExistingKBGroupJuridicalPersonExposures ?? _emptyExistingKBGroupItems))
        {
            var itemToAdd = addExistingKbCodebtors(exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true), otherCustomers!, exp.BankAccount);
            obligations.Add(itemToAdd);
        }

        // zavazky NOBY
        obligations.AddRange(nobyObligations.CreateHouseholdObligations(_obligationTypes));

        return obligations
            .OrderBy(t => t.ObligationSourceId)
            .ThenBy(t => t.ObligationTypeOrder)
            .ToList();
    }

    // najit vsechny zavazky, ktere ma tento klient spolecne s ostatnimi klienty v exposure KB
    private Dto.HouseholdObligationItem addRequestedKbCodebtors(Dto.HouseholdObligationItem item, List<CustomerExposureCustomer> otherCustomers, string? riskBusinessCaseId)
    {
        if (!string.IsNullOrEmpty(riskBusinessCaseId))
        {
            foreach (var otherCustomer in otherCustomers)
            {
                var itemToUnion1 = otherCustomer.RequestedKBGroupNaturalPersonExposures?.FirstOrDefault(o => o.RiskBusinessCaseId == riskBusinessCaseId);
                if (itemToUnion1 is not null)
                {
                    otherCustomer.RequestedKBGroupNaturalPersonExposures!.Remove(itemToUnion1);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }

                var itemToUnion2 = otherCustomer.RequestedKBGroupJuridicalPersonExposures?.FirstOrDefault(o => o.RiskBusinessCaseId == riskBusinessCaseId);
                if (itemToUnion2 is not null)
                {
                    otherCustomer.RequestedKBGroupJuridicalPersonExposures!.Remove(itemToUnion2);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }
            }
        }
        return item;
    }

    // najit vsechny zavazky, ktere ma tento klient spolecne s ostatnimi klienty v exposure CBCB
    private Dto.HouseholdObligationItem addRequestedCbCbCodebtors(Dto.HouseholdObligationItem item, List<CustomerExposureCustomer> otherCustomers, string? cbcbContractId)
    {
        if (!string.IsNullOrEmpty(cbcbContractId))
        {
            foreach (var otherCustomer in otherCustomers)
            {
                var itemToUnion1 = otherCustomer.RequestedCBCBNaturalPersonExposureItem?.FirstOrDefault(o => o.CbcbContractId == cbcbContractId);
                if (itemToUnion1 is not null)
                {
                    otherCustomer.RequestedCBCBNaturalPersonExposureItem!.Remove(itemToUnion1);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }

                var itemToUnion2 = otherCustomer.RequestedCBCBJuridicalPersonExposureItem?.FirstOrDefault(o => o.CbcbContractId == cbcbContractId);
                if (itemToUnion2 is not null)
                {
                    otherCustomer.RequestedCBCBJuridicalPersonExposureItem!.Remove(itemToUnion2);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }
            }
        }
        return item;
    }

    // najit vsechny zavazky, ktere ma tento klient spolecne s ostatnimi klienty v exposure KB
    private Dto.HouseholdObligationItem addExistingKbCodebtors(Dto.HouseholdObligationItem item, List<CustomerExposureCustomer> otherCustomers, BankAccountDetail? bankAccount)
    {
        if (bankAccount is not null)
        {
            foreach (var otherCustomer in otherCustomers)
            {
                var itemToUnion1 = otherCustomer.ExistingKBGroupNaturalPersonExposures?.FirstOrDefault(o => bankAccount.BankAccountEquals(o.BankAccount));
                if (itemToUnion1 is not null)
                {
                    otherCustomer.ExistingKBGroupNaturalPersonExposures!.Remove(itemToUnion1);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }

                var itemToUnion2 = otherCustomer.ExistingKBGroupJuridicalPersonExposures?.FirstOrDefault(o => bankAccount.BankAccountEquals(o.BankAccount));
                if (itemToUnion2 is not null)
                {
                    otherCustomer.ExistingKBGroupJuridicalPersonExposures!.Remove(itemToUnion2);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }
            }
        }
        return item;
    }

    // najit vsechny zavazky, ktere ma tento klient spolecne s ostatnimi klienty v exposure CBCB
    private Dto.HouseholdObligationItem addExistingCbCbCodebtors(Dto.HouseholdObligationItem item, List<CustomerExposureCustomer> otherCustomers, string? cbcbContractId)
    {
        if (!string.IsNullOrEmpty(cbcbContractId))
        {
            foreach (var otherCustomer in otherCustomers)
            {
                var itemToUnion1 = otherCustomer.ExistingCBCBNaturalPersonExposureItem?.FirstOrDefault(o => o.CbcbContractId == cbcbContractId);
                if (itemToUnion1 is not null)
                {
                    otherCustomer.ExistingCBCBNaturalPersonExposureItem!.Remove(itemToUnion1);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }

                var itemToUnion2 = otherCustomer.ExistingCBCBJuridicalPersonExposureItem?.FirstOrDefault(o => o.CbcbContractId == cbcbContractId);
                if (itemToUnion2 is not null)
                {
                    otherCustomer.ExistingCBCBJuridicalPersonExposureItem!.Remove(itemToUnion2);
                    addCodebtorNames(item, otherCustomer.InternalCustomerId!.Value);
                }
            }
        }
        return item;
    }

    private void addCodebtorNames(Dto.HouseholdObligationItem item, long internalCustomerId)
    {
        var c = _customers.First(t => t.CustomerOnSAId == internalCustomerId);

        item.Codebtors ??= new List<string>();
        item.Codebtors.Add($"{c.FirstNameNaturalPerson} {c.Name}");

    }

    // empty arrays for easily readable code (imo :-))
    private static List<CustomerExposureRequestedCBCBItem> _emptyRequestedCBCBGroupItems = new(0);
    private static List<CustomerExposureRequestedKBGroupItem> _emptyRequestedKBGroupItems = new(0);
    private static List<CustomerExposureExistingKBGroupItem> _emptyExistingKBGroupItems = new(0);
    private static List<CustomerExposureExistingCBCBItem> _emptyExistingCBCBGroupItems = new(0);

    // exec time props
    private List<CustomerOnSA> _customers = null!;
    private Dictionary<int, List<Obligation>> _obligations = null!;
    private List<ObligationTypesResponse.Types.ObligationTypeItem> _obligationTypes = null!;
    private List<ObligationLaExposuresResponse.Types.ObligationLaExposureItem> _laExposureItems = null!;

    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient _customerOnSAService;
    private readonly DomainServices.HouseholdService.Clients.IHouseholdServiceClient _householdService;

    public GetLoanApplicationAssessmentResultService(
        DomainServices.HouseholdService.Clients.ICustomerOnSAServiceClient customerOnSAService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        ICurrentUserAccessor currentUser,
        DomainServices.HouseholdService.Clients.IHouseholdServiceClient householdService)
    {
        _codebookService = codebookService;
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _currentUser = currentUser;
    }
}
