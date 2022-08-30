using CIS.Infrastructure.gRPC.CisTypes;

using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CustomerService.Abstraction;
using DomainServices.UserService.Abstraction;

using cArrangement = DomainServices.SalesArrangementService.Contracts;
using cCase = DomainServices.CaseService.Contracts;
using cOffer = DomainServices.OfferService.Contracts;
using cCustomer = DomainServices.CustomerService.Contracts;
using cUser = DomainServices.UserService.Contracts;

namespace FOMS.Api.Endpoints.SalesArrangement.GetLoanApplicationAssessment;

[CIS.Infrastructure.Attributes.ScopedService, CIS.Infrastructure.Attributes.SelfService]
internal class LoanApplicationDataService
{

    #region Construction

    private static readonly string StringJoinSeparator = ",";

    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly ICustomerOnSAServiceAbstraction _customerOnSAService;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly IUserServiceAbstraction _userService;
    private readonly ICustomerServiceAbstraction _customerService;

    public LoanApplicationDataService(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        IOfferServiceAbstraction offerService,
        ISalesArrangementServiceAbstraction salesArrangementService,
        ICustomerOnSAServiceAbstraction customerOnSAService,
        IHouseholdServiceAbstraction householdService,
        ICaseServiceAbstraction caseService,
        IUserServiceAbstraction userService,
        ICustomerServiceAbstraction customerService
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
    }

    #endregion

    #region Data loading

    public static string IdentityToCode(Identity identity)
    {
        return $"{identity.IdentityScheme}|{identity.IdentityId}";
    }

    private async Task<List<cArrangement.Household>> GetHouseholds(int salesArrangementId, CancellationToken cancellation)
    {
        var householdList = ServiceCallResult.ResolveAndThrowIfError<List<cArrangement.Household>>(await _householdService.GetHouseholdList(salesArrangementId, cancellation));
        var householdIds = householdList.Select(i => i.HouseholdId).ToArray();

        var households = new List<cArrangement.Household>();
        for (int i = 0; i < householdIds.Length; i++)
        {
            var household = ServiceCallResult.ResolveAndThrowIfError<cArrangement.Household>(await _householdService.GetHousehold(householdIds[i], cancellation));
            households.Add(household);
        }
        return households;
    }

    private async Task<List<cArrangement.CustomerOnSA>> GetCustomersOnSA(int salesArrangementId, CancellationToken cancellation)
    {
        var customerOnSAList = ServiceCallResult.ResolveAndThrowIfError<List<cArrangement.CustomerOnSA>>(await _customerOnSAService.GetCustomerList(salesArrangementId, cancellation));
        var customerOnSAIds = customerOnSAList.Select(i => i.CustomerOnSAId).ToArray();

        var customers = new List<cArrangement.CustomerOnSA>();
        for (int i = 0; i < customerOnSAIds.Length; i++)
        {
            var customer = ServiceCallResult.ResolveAndThrowIfError<cArrangement.CustomerOnSA>(await _customerOnSAService.GetCustomer(customerOnSAIds[i], cancellation));
            customers.Add(customer);
        }
        return customers;
    }

    private async Task<Dictionary<int, cArrangement.Income>> GetIncomesById(List<cArrangement.CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(i => i.IncomeId)).ToArray();
        var incomes = new List<cArrangement.Income>();
        for (int i = 0; i < incomeIds.Length; i++)
        {
            var income = ServiceCallResult.ResolveAndThrowIfError<cArrangement.Income>(await _customerOnSAService.GetIncome(incomeIds[i], cancellation));
            incomes.Add(income);
        }
        return incomes.ToDictionary(i => i.IncomeId);
    }

    private async Task<Dictionary<string, cCustomer.CustomerResponse>> GetCustomersByIdentityCode(List<cArrangement.CustomerOnSA> customersOnSa, CancellationToken cancellation)
    {
        // vrací pouze pro KB identity
        var customerIdentities = customersOnSa.SelectMany(i => i.CustomerIdentifiers.Where(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)).GroupBy(i => IdentityToCode(i)).Select(i => i.First()).ToList();
        var results = new List<cCustomer.CustomerResponse>();
        for (int i = 0; i < customerIdentities.Count; i++)
        {
            var customer = ServiceCallResult.ResolveAndThrowIfError<cCustomer.CustomerResponse>(await _customerService.GetCustomerDetail(new cCustomer.CustomerRequest { Identity = customerIdentities[i] }, cancellation));
            results.Add(customer!);
        }
        return results.ToDictionary(i => IdentityToCode( i.Identities.First()));
    }

    #endregion

    public async Task<LoanApplicationData> LoadData(int salesArrangementId, CancellationToken cancellation)
    {
        var arrangement = ServiceCallResult.ResolveAndThrowIfError<cArrangement.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellation));
        var offer = ServiceCallResult.ResolveAndThrowIfError<cOffer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(arrangement.OfferId!.Value, cancellation));
        var customersOnSA = await GetCustomersOnSA(arrangement.SalesArrangementId, cancellation);
        var households = await GetHouseholds(arrangement.SalesArrangementId, cancellation);
        var incomesById = await GetIncomesById(customersOnSA, cancellation);
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<cCase.Case>(await _caseService.GetCaseDetail(arrangement.CaseId, cancellation));
        var customersByIdentityCode = await GetCustomersByIdentityCode(customersOnSA, cancellation);
        var user = ServiceCallResult.ResolveAndThrowIfError<cUser.User>(await _userService.GetUser(_userAccessor.User!.Id, cancellation));
        return new LoanApplicationData(arrangement, offer, user, caseInstance, households, customersOnSA, incomesById, customersByIdentityCode);
    }

}
