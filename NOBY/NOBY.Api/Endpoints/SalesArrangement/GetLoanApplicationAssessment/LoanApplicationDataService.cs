using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.SalesArrangementService.Clients;
using DomainServices.CaseService.Clients;
using DomainServices.OfferService.Clients;
using DomainServices.UserService.Clients;

using cSA = DomainServices.SalesArrangementService.Contracts;
using cHousehold = DomainServices.HouseholdService.Contracts;
using cCase = DomainServices.CaseService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCustomer = DomainServices.CustomerService.Contracts;
using cUser = DomainServices.UserService.Contracts;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Clients;

namespace NOBY.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal class LoanApplicationDataService
{

    #region Construction

    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly IOfferServiceClients _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICaseServiceClient _caseService;
    private readonly IUserServiceClient _userService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICodebookServiceClients _codebookService;

    public LoanApplicationDataService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        IOfferServiceClients offerService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSAService,
        IHouseholdServiceClient householdService,
        ICaseServiceClient caseService,
        IUserServiceClient userService,
        ICustomerServiceClient customerService,
        ICodebookServiceClients codebookService
        )
    {
        _userAccessor = userAccessor;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
        _customerOnSAService = customerOnSAService;
        _householdService = householdService;
        _caseService = caseService;
        _userService = userService;
        _customerService = customerService;
        _codebookService = codebookService;
    }

    #endregion

    #region Data loading

    public static string IdentityToCode(Identity identity)
    {
        return $"{identity.IdentityScheme}|{identity.IdentityId}";
    }

    private async Task<List<cHousehold.Household>> GetHouseholds(int salesArrangementId, CancellationToken cancellation)
    {
        var householdList = await _householdService.GetHouseholdList(salesArrangementId, cancellation);
        var householdIds = householdList.Select(i => i.HouseholdId).ToArray();

        var households = new List<cHousehold.Household>();
        for (int i = 0; i < householdIds.Length; i++)
        {
            var household = await _householdService.GetHousehold(householdIds[i], cancellation);
            households.Add(household);
        }
        return households;
    }

    private async Task<List<cHousehold.CustomerOnSA>> GetCustomersOnSA(int salesArrangementId, CancellationToken cancellation)
    {
        var customerOnSAList = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellation);
        var customerOnSAIds = customerOnSAList.Select(i => i.CustomerOnSAId).ToArray();

        var customers = new List<cHousehold.CustomerOnSA>();
        for (int i = 0; i < customerOnSAIds.Length; i++)
        {
            var customer = await _customerOnSAService.GetCustomer(customerOnSAIds[i], cancellation);
            customers.Add(customer);
        }
        return customers;
    }

    private async Task<Dictionary<int, cHousehold.Income>> GetIncomesById(List<cHousehold.CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(i => i.IncomeId)).ToArray();
        var incomes = new List<cHousehold.Income>();
        for (int i = 0; i < incomeIds.Length; i++)
        {
            var income = await _customerOnSAService.GetIncome(incomeIds[i], cancellation);
            incomes.Add(income);
        }
        return incomes.ToDictionary(i => i.IncomeId);
    }

    private async Task<Dictionary<string, cCustomer.CustomerDetailResponse>> GetCustomersByIdentityCode(List<cHousehold.CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        // vrací pouze pro KB identity
        var customerIdentities = customersOnSa.SelectMany(i => i.CustomerIdentifiers.Where(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)).GroupBy(i => IdentityToCode(i)).Select(i => i.First()).ToList();
        var results = new List<cCustomer.CustomerDetailResponse>();
        for (int i = 0; i < customerIdentities.Count; i++)
        {
            var customer = ServiceCallResult.ResolveAndThrowIfError<cCustomer.CustomerDetailResponse>(await _customerService.GetCustomerDetail(customerIdentities[i], cancellation));
            results.Add(customer!);
        }
        return results.ToDictionary(i => IdentityToCode(i.Identity));
    }

    #endregion

    public async Task<LoanApplicationData> LoadData(int salesArrangementId, CancellationToken cancellation)
    {
        var arrangement = ServiceCallResult.ResolveAndThrowIfError<cSA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellation));
        var offer = ServiceCallResult.ResolveAndThrowIfError<cOffer.GetMortgageOfferDetailResponse>(await _offerService.GetMortgageOfferDetail(arrangement.OfferId!.Value, cancellation));
        var customersOnSA = await GetCustomersOnSA(arrangement.SalesArrangementId, cancellation);
        var households = await GetHouseholds(arrangement.SalesArrangementId, cancellation);
        var incomesById = await GetIncomesById(customersOnSA, cancellation);
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<cCase.Case>(await _caseService.GetCaseDetail(arrangement.CaseId, cancellation));
        var customersByIdentityCode = await GetCustomersByIdentityCode(customersOnSA, cancellation);
        //Dictionary<string, cCustomer.CustomerDetailResponse> customersByIdentityCode = null;
        var user = await _userService.GetUser(_userAccessor.User!.Id, cancellation);
        var academicDegreesBefore = await _codebookService.AcademicDegreesBefore(cancellation);
        var countries = await _codebookService.Countries(cancellation);
        var obligationTypes = await _codebookService.ObligationTypes(cancellation);
        return new LoanApplicationData(arrangement, offer, user, caseInstance, households, customersOnSA, incomesById, customersByIdentityCode, academicDegreesBefore, countries, obligationTypes);
    }

}
