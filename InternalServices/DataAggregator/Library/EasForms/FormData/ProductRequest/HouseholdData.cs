﻿using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest;

[TransientService, SelfService]
internal class HouseholdData : IHouseholdData
{
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerServiceClient _customerService;

    private Dictionary<long, CustomerDetailResponse> _customers = null!;
    private Dictionary<int, string> _academicDegreesBefore = null!;
    private Dictionary<int, string> _genders = null!;
    private Dictionary<int, string> _countries = null!;
    private ILookup<string, int> _obligationTypes = null!;

    private int _firstEmploymentTypeId;

    public HouseholdData(ICustomerOnSAServiceClient customerOnSaService, IHouseholdServiceClient householdService, ICustomerServiceClient customerService)
    {
        _customerOnSaService = customerOnSaService;
        _householdService = householdService;
        _customerService = customerService;
    }

    public Household Household { get; private set; } = null!;

    public List<CustomerOnSA> CustomersOnSa { get; private set; } = null!;

    public List<Household> Households { get; private set; } = null!;

    public Dictionary<int, Income> Incomes { get; private set; } = null!;

    public IEnumerable<Customer> Customers => GetCustomers();

    public async Task Initialize(int salesArrangementId)
    {
        CustomersOnSa = await LoadCustomersOnSA(salesArrangementId);
        Households = await LoadHouseholds(salesArrangementId);
        _customers = await LoadCustomers(CustomersOnSa);
        Incomes = await LoadIncomes(CustomersOnSa);

        if (!TrySetHousehold(HouseholdTypes.Main))
            throw new InvalidOperationException();
    }

    public async Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        _academicDegreesBefore = (await codebookService.AcademicDegreesBefore()).ToDictionary(a => a.Id, a => a.Name);
        _genders = (await codebookService.Genders()).ToDictionary(g => g.Id, g => g.StarBuildJsonCode);
        _countries = (await codebookService.Countries()).ToDictionary(c => c.Id, c => c.ShortName);
        _firstEmploymentTypeId = (await codebookService.EmploymentTypes()).OrderBy(e => e.Id).Select(e => e.Id).FirstOrDefault();
        _obligationTypes = (await codebookService.ObligationTypes()).ToLookup(o => o.ObligationProperty, o => o.Id);
    }
    
    public bool TrySetHousehold(HouseholdTypes householdType)
    {
        var household = Households.FirstOrDefault(h => h.HouseholdType == householdType);

        if (household is null)
            return false;

        Household = household;

        return true;
    }

    private async Task<List<Household>> LoadHouseholds(int salesArrangementId)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId);

        return households.OrderBy(x => x.HouseholdTypeId)
                         .Select((household, index) => new Household(household, index + 1))
                         .ToList();
    }


    private async Task<List<CustomerOnSA>> LoadCustomersOnSA(int salesArrangementId)
    {
        var customers = await _customerOnSaService.GetCustomerList(salesArrangementId);

        var customersWithDetail = new List<CustomerOnSA>(customers.Count);

        foreach (var customerId in customers.Select(c => c.CustomerOnSAId))
        {
            var customerDetail = await _customerOnSaService.GetCustomer(customerId);
            customersWithDetail.Add(customerDetail);
        }
        return customersWithDetail;
    }

    private async Task<Dictionary<long, CustomerDetailResponse>> LoadCustomers(IEnumerable<CustomerOnSA> customersOnSa)
    {
        var customerIds = customersOnSa.SelectMany(c => c.CustomerIdentifiers)
                                       .Where(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb)
                                       .Select(i => i.IdentityId)
                                       .Distinct();

        var result = await _customerService.GetCustomerList(customerIds.Select(id => new Identity(id, IdentitySchemes.Kb)));

        return result.Customers.ToDictionary(c => c.Identities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId, c => c);
    }

    private async Task<Dictionary<int, Income>> LoadIncomes(IEnumerable<CustomerOnSA> customersOnSa)
    {
        var incomeIds = customersOnSa.SelectMany(i => i.Incomes.Select(x => x.IncomeId)).ToList();

        var incomes = new Dictionary<int, Income>(incomeIds.Count);

        foreach (var incomeId in incomeIds)
        {
            var income = await _customerOnSaService.GetIncome(incomeId);

            incomes.Add(incomeId, income);
        }

        return incomes;
    }

    private IEnumerable<Customer> GetCustomers()
    {
        return CustomersOnSa.Where(c => c.CustomerOnSAId == Household.CustomerOnSaId1 || c.CustomerOnSAId == Household.CustomerOnSaId2)
                            .OrderBy(c => c.CustomerOnSAId)
                            .Select(c => new Customer(c, _customers[GetCustomerId(c)])
                            {
                                HouseholdNumber = Household.Number,
                                IsPartner = AreCustomersPartners(),
                                FirstEmploymentTypeId = _firstEmploymentTypeId,
                                Incomes = Incomes,
                                AcademicDegreesBefore = _academicDegreesBefore,
                                GenderCodes = _genders,
                                Countries = _countries,
                                ObligationTypes = _obligationTypes
                            }).ToList();

        static long GetCustomerId(CustomerOnSA customerOnSa) =>
            customerOnSa.CustomerIdentifiers.Single(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId;
    }

    private bool AreCustomersPartners() =>
        CustomersOnSa.Count == 2 &&
        DomainServices.HouseholdService.Clients.Helpers.AreCustomersPartners(CustomersOnSa[0].MaritalStatusId, CustomersOnSa[1].MaritalStatusId);
}