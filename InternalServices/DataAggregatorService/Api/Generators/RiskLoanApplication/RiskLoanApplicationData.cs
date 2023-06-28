using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.RiskLoanApplication;

internal class RiskLoanApplicationData : AggregatedData
{
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;

    public RiskLoanApplicationData(IHouseholdServiceClient householdService, ICustomerOnSAServiceClient customerOnSAService, ICustomerServiceClient customerService)
    {
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
    }

    public List<LoanApplicationHousehold> Households { get; private set; } = new();

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(SalesArrangement.SalesArrangementId, cancellationToken);
        var customersOnSa = await LoadCustomersOnSA(SalesArrangement.SalesArrangementId, cancellationToken);
        var customers = await LoadCustomers(customersOnSa.Values, cancellationToken);
        var incomes = await LoadIncomes(customersOnSa.Values, cancellationToken);

        Households = households.Select(household =>
        {
            var householdCustomers = new[] { customersOnSa.GetValueOrDefault(household.CustomerOnSAId1 ?? 0), customersOnSa.GetValueOrDefault(household.CustomerOnSAId2 ?? 0) }.OfType<CustomerOnSA>().ToArray();

            var areCustomersPartners = householdCustomers.Length == 2 && DomainServices.HouseholdService.Clients.Helpers.AreCustomersPartners(householdCustomers[0].MaritalStatusId, householdCustomers[1].MaritalStatusId);

            return new LoanApplicationHousehold
            {
                Household = household,
                Customers = householdCustomers.Select(customerOnSA =>
                                              {
                                                  var customerDetail = customers[customerOnSA.CustomerIdentifiers.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId];

                                                  return new LoanApplicationCustomer(customerOnSA, customerDetail, incomes, _codebookManager.DegreesBefore)
                                                  {
                                                      IsPartner = areCustomersPartners,
                                                      ObligationTypes = _codebookManager.ObligationTypes.GroupBy(i => i.ObligationProperty).ToDictionary(i => i.Key, l => l.Select(i => i.Id).ToList())
                                                  };
                                              }).ToList()
            };
        }).ToList();
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore().ObligationTypes();
    }

    private async Task<Dictionary<int, CustomerOnSA>> LoadCustomersOnSA(int salesArrangementId, CancellationToken cancellationToken)
    {
        var customers = await _customerOnSAService.GetCustomerList(salesArrangementId, cancellationToken);

        var customersWithDetail = new Dictionary<int, CustomerOnSA>(customers.Count);

        foreach (var customerId in customers.Where(c => c.CustomerIdentifiers.Any(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)).Select(c => c.CustomerOnSAId))
        {
            var customerDetail = await _customerOnSAService.GetCustomer(customerId, cancellationToken);
            customersWithDetail.Add(customerId, customerDetail);
        }

        return customersWithDetail;
    }

    private async Task<Dictionary<long, CustomerDetailResponse>> LoadCustomers(IEnumerable<CustomerOnSA> customersOnSa, CancellationToken cancellationToken)
    {
        var customerIds = customersOnSa.SelectMany(c => c.CustomerIdentifiers)
                                       .Where(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
                                       .Select(i => i.IdentityId)
                                       .Distinct();

        var result = await _customerService.GetCustomerList(customerIds.Select(id => new Identity(id, IdentitySchemes.Kb)), cancellationToken);

        return result.Customers.ToDictionary(c => c.Identities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId, c => c);
    }

    private async Task<Dictionary<int, Income>> LoadIncomes(IEnumerable<CustomerOnSA> customersOnSa, CancellationToken cancellationToken)
    {
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(x => x.IncomeId)).ToList();

        var incomes = new Dictionary<int, Income>(incomeIds.Count);

        foreach (var incomeId in incomeIds)
        {
            var income = await _customerOnSAService.GetIncome(incomeId, cancellationToken);

            incomes.Add(incomeId, income);
        }

        return incomes;
    }
}