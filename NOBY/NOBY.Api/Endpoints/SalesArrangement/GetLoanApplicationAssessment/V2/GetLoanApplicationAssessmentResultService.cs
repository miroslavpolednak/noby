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

        // zjisteni spoludluzniku a smazani jejich pripadnych zavazku ve prospech hlavniho dluznika
        _codebtors = exposure is null ? new() : findAndProcessCodebtors(exposure);

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

    private Dictionary<int, List<string>> findAndProcessCodebtors(CustomerExposureCalculateResponse exposure)
    {
        List<ExposureItemsHelperDto> items = new();
        exposure!.Customers!.ForEach(t =>
        {
            int customerId = t.GetHashCode();

            t.RequestedCBCBNaturalPersonExposureItem ??= new(0);
            t.RequestedCBCBJuridicalPersonExposureItem ??= new(0);
            t.RequestedKBGroupNaturalPersonExposures ??= new(0);
            t.RequestedKBGroupJuridicalPersonExposures ??= new(0);
            t.ExistingCBCBNaturalPersonExposureItem ??= new(0);
            t.ExistingCBCBJuridicalPersonExposureItem ??= new(0);
            t.ExistingKBGroupNaturalPersonExposures ??= new(0);
            t.ExistingKBGroupJuridicalPersonExposures ??= new(0);

            if (t.RequestedCBCBNaturalPersonExposureItem.Count != 0)
            {
                items.AddRange(t.RequestedCBCBNaturalPersonExposureItem!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), (x.CbcbContractId ?? ""), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.RequestedCBCBJuridicalPersonExposureItem.Count != 0)
            {
                items.AddRange(t.RequestedCBCBJuridicalPersonExposureItem!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), (x.CbcbContractId ?? ""), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.RequestedKBGroupNaturalPersonExposures.Count != 0)
            {
                items.AddRange(t.RequestedKBGroupNaturalPersonExposures!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), (x.RiskBusinessCaseId ?? ""), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.RequestedKBGroupJuridicalPersonExposures.Count != 0)
            {
                items.AddRange(t.RequestedKBGroupJuridicalPersonExposures!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), (x.RiskBusinessCaseId ?? ""), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.ExistingCBCBNaturalPersonExposureItem.Count != 0)
            {
                items.AddRange(t.ExistingCBCBNaturalPersonExposureItem!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), (x.CbcbContractId ?? ""), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.ExistingCBCBJuridicalPersonExposureItem.Count != 0)
            {
                items.AddRange(t.ExistingCBCBJuridicalPersonExposureItem!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), (x.CbcbContractId ?? ""), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.ExistingKBGroupNaturalPersonExposures.Count != 0)
            {
                items.AddRange(t.ExistingKBGroupNaturalPersonExposures!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), getBankAccount(x.BankAccount), x.CustomerRoleId.GetValueOrDefault())));
            }
            if (t.ExistingKBGroupJuridicalPersonExposures.Count != 0)
            {
                items.AddRange(t.ExistingKBGroupJuridicalPersonExposures!.Select(x => new ExposureItemsHelperDto(customerId, x.GetHashCode(), getBankAccount(x.BankAccount), x.CustomerRoleId.GetValueOrDefault())));
            }

            string getBankAccount(BankAccountDetail? bankAccount)
                => bankAccount is null ? "" : $"{bankAccount.NumberPrefix}-{bankAccount.Number}/{bankAccount.BankCode}";
        });

        Dictionary<int, List<string>> codebtors = new();

        items
            .GroupBy(t => t.Account)
            .Where(t => t.Count() > 1)
            .ToList()
            .ForEach(t =>
            {
                var c = t.FirstOrDefault(x => x.Role == 1) ?? t.First();
                codebtors.TryAdd(c.Hash, new());

                foreach (var other in t.Where(x => x.Hash != c.Hash))
                {
                    var otherCustomer = exposure.Customers.First(x => x.GetHashCode() == other.CustomerHash);

                    var oc = _customers.First(t => t.CustomerOnSAId == otherCustomer.InternalCustomerId!.Value);
                    codebtors[c.Hash].Add($"{oc.FirstNameNaturalPerson} {oc.Name}");

                    otherCustomer.RequestedCBCBNaturalPersonExposureItem!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.RequestedCBCBJuridicalPersonExposureItem!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.RequestedKBGroupNaturalPersonExposures!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.RequestedKBGroupJuridicalPersonExposures!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.ExistingCBCBNaturalPersonExposureItem!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.ExistingCBCBJuridicalPersonExposureItem!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.ExistingKBGroupNaturalPersonExposures!.RemoveAll(x => x.GetHashCode() == other.Hash);
                    otherCustomer.ExistingKBGroupJuridicalPersonExposures!.RemoveAll(x => x.GetHashCode() == other.Hash);
                }
            });

        return codebtors;
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
            RoleId = (CustomerRoles)customer.CustomerRoleId
        };
        
        if (customerExposure is null)
        {
            obligationCustomer.ExistingObligations = obligations.CreateHouseholdObligations(_obligationTypes)
                .OrderBy(t => t.ObligationTypeOrder)
                .ToList();
        }
        else
        {
            obligationCustomer.ExistingObligations = getExistingObligations(customerExposure, obligations);

            if (_currentUser.HasPermission(UserPermissions.CLIENT_EXPOSURE_DisplayRequestedExposure))
            {
                obligationCustomer.RequestedObligations = getRequestedObligations(customerExposure);
            }
        }
        
        return obligationCustomer;
    }

    private List<Dto.HouseholdObligationItem> getRequestedObligations(CustomerExposureCustomer customerExposure)
    {
        var obligations = new List<Dto.HouseholdObligationItem>();

        foreach (var exp in customerExposure.RequestedCBCBNaturalPersonExposureItem!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        foreach (var exp in customerExposure.RequestedCBCBJuridicalPersonExposureItem!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        foreach (var exp in customerExposure.RequestedKBGroupNaturalPersonExposures!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        foreach (var exp in customerExposure.RequestedKBGroupJuridicalPersonExposures!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        return obligations
            .OrderBy(t => t.ObligationTypeOrder)
            .ToList();
    }

    private List<Dto.HouseholdObligationItem> getExistingObligations(
        CustomerExposureCustomer customerExposure, 
        List<Obligation> nobyObligations)
    {
        var obligations = new List<Dto.HouseholdObligationItem>();
        
        foreach (var exp in customerExposure.ExistingCBCBNaturalPersonExposureItem!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        foreach (var exp in customerExposure.ExistingCBCBJuridicalPersonExposureItem!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }
    
        foreach (var exp in customerExposure.ExistingKBGroupNaturalPersonExposures!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, false);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        foreach (var exp in customerExposure.ExistingKBGroupJuridicalPersonExposures!)
        {
            var item = exp.CreateHouseholdObligations(_laExposureItems, _obligationTypes, true);
            obligations.Add(updateCodebtors(item, exp.GetHashCode()));
        }

        // zavazky NOBY
        obligations.AddRange(nobyObligations.CreateHouseholdObligations(_obligationTypes));

        return obligations
            .OrderBy(t => t.ObligationSourceId)
            .ThenBy(t => t.ObligationTypeOrder)
            .ToList();
    }

    private Dto.HouseholdObligationItem updateCodebtors(Dto.HouseholdObligationItem item, int hash)
    {
        if (_codebtors!.TryGetValue(hash, out List<string>? value))
        {
            item.Codebtors = value;
        }
        return item;
    }

    private sealed record ExposureItemsHelperDto(int CustomerHash, int Hash, string Account, int Role) { }

    // [int = hash exposure.Customer.obligationItem[] objektu, List<> = jmena spoludluzniku]
    private Dictionary<int, List<string>>? _codebtors;

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
