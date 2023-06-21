using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.CodebookService.Contracts.v1;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest;

[TransientService, SelfService]
internal class HouseholdData
{
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerServiceClient _customerService;
    private readonly ICustomerChangeDataMerger _customerChangeDataMerger;

    private Dictionary<long, CustomerDetailResponse> _customers = null!;
    private Dictionary<int, string> _academicDegreesBefore = null!;
    private Dictionary<int, string> _genders = null!;
    private ILookup<string, int> _obligationTypes = null!;
    private List<GenericCodebookResponse.Types.GenericCodebookItem> _legalCapacityTypes = null!;

    private int _firstEmploymentTypeId;

    public HouseholdData(ICustomerOnSAServiceClient customerOnSaService, IHouseholdServiceClient householdService, ICustomerServiceClient customerService, ICustomerChangeDataMerger customerChangeDataMerger)
    {
        _customerOnSaService = customerOnSaService;
        _householdService = householdService;
        _customerService = customerService;
        _customerChangeDataMerger = customerChangeDataMerger;
    }

    public HouseholdDto HouseholdDto { get; private set; } = null!;

    public List<CustomerOnSA> CustomersOnSa { get; private set; } = null!;

    public List<HouseholdDto> Households { get; private set; } = null!;

    public Dictionary<int, Income> Incomes { get; private set; } = null!;

    public IEnumerable<Customer> Customers { get; private set; } = Enumerable.Empty<Customer>();

    public bool? IsSpouseInDebt { get; set; }

    public async Task Initialize(int salesArrangementId, CancellationToken cancellationToken)
    {
        CustomersOnSa = await LoadCustomersOnSA(salesArrangementId, cancellationToken);

        await Task.WhenAll(LoadHouseholdsWrapper(), LoadCustomersWrapper(), LoadIncomesWrapper());

        SetHouseholdData(Households.Select(h => h.HouseholdId).FirstOrDefault());

        async Task LoadHouseholdsWrapper() => Households = await LoadHouseholds(salesArrangementId, cancellationToken);
        async Task LoadCustomersWrapper() => _customers = await LoadCustomers(CustomersOnSa, cancellationToken);
        async Task LoadIncomesWrapper() => Incomes = await LoadIncomes(CustomersOnSa, cancellationToken);
    }

    public void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore().Genders().EmploymentTypes().ObligationTypes().LegalCapacityRestrictionTypes();
    }

    public void PrepareCodebooks(CodebookManager codebookManager)
    {
        _academicDegreesBefore = codebookManager.DegreesBefore.ToDictionary(a => a.Id, a => a.Name);
        _genders = codebookManager.Genders.ToDictionary(g => g.Id, g => g.StarBuildJsonCode);
        _firstEmploymentTypeId = codebookManager.EmploymentTypes.OrderBy(e => e.Id).Select(e => e.Id).FirstOrDefault();
        _obligationTypes =codebookManager.ObligationTypes.ToLookup(o => o.ObligationProperty, o => o.Id);
        _legalCapacityTypes = codebookManager.LegalCapacityRestrictionTypes;
    }
    
    public void SetHouseholdData(int householdId)
    {
        var household = Households.FirstOrDefault(h => h.HouseholdId == householdId);

        HouseholdDto = household ?? throw new InvalidOperationException($"Requested Household ({householdId}) was not found.");

        Customers = GetCustomers().ToList();
    }

    private async Task<List<HouseholdDto>> LoadHouseholds(int salesArrangementId, CancellationToken cancellationToken)
    {
        var households = await _householdService.GetHouseholdList(salesArrangementId, cancellationToken);

        return households.OrderBy(x => x.HouseholdTypeId)
                         .Select((household, index) => new HouseholdDto(household, index + 1))
                         .ToList();
    }


    private async Task<List<CustomerOnSA>> LoadCustomersOnSA(int salesArrangementId, CancellationToken cancellationToken)
    {
        var customers = await _customerOnSaService.GetCustomerList(salesArrangementId, cancellationToken);

        var customersWithDetail = new List<CustomerOnSA>(customers.Count);

        foreach (var customerId in customers.Select(c => c.CustomerOnSAId))
        {
            var customerDetail = await _customerOnSaService.GetCustomer(customerId, cancellationToken);
            customersWithDetail.Add(customerDetail);
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
            var income = await _customerOnSaService.GetIncome(incomeId, cancellationToken);

            incomes.Add(incomeId, income);
        }

        return incomes;
    }

    private IEnumerable<Customer> GetCustomers()
    {
        var customersOnSa = CustomersOnSa.Where(c => c.CustomerOnSAId == HouseholdDto.CustomerOnSaId1 || c.CustomerOnSAId == HouseholdDto.CustomerOnSaId2).OrderBy(c => c.CustomerOnSAId);

        foreach (var customerOnSA in customersOnSa)
        {
            var customerDetail = _customers[GetCustomerId(customerOnSA)];

            _customerChangeDataMerger.MergeAll(customerDetail, customerOnSA);

            yield return new Customer(customerOnSA, customerDetail)
            {
                HouseholdNumber = HouseholdDto.Number,
                IsPartner = AreCustomersPartners(),
                FirstEmploymentTypeId = _firstEmploymentTypeId,
                Incomes = Incomes,
                AcademicDegreesBefore = _academicDegreesBefore,
                GenderCodes = _genders,
                ObligationTypes = _obligationTypes,
                LegalCapacityTypes = _legalCapacityTypes,
                IsSpouseInDebt = IsSpouseInDebt
            };
        }

        static long GetCustomerId(CustomerOnSA customerOnSa)
        {
            var customerIdentity = customerOnSa.CustomerIdentifiers.SingleOrDefault(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb) ??
                           throw new InvalidOperationException($"CustomerOnSa {customerOnSa.CustomerOnSAId} does not have KB ID");

            return customerIdentity.IdentityId;
        }
    }

    private bool AreCustomersPartners()
    {
        var householdCustomers = CustomersOnSa.Where(c => c.CustomerOnSAId == HouseholdDto.CustomerOnSaId1 || c.CustomerOnSAId == HouseholdDto.CustomerOnSaId2).ToArray();

        return householdCustomers.Length == 2 &&
               DomainServices.HouseholdService.Clients.Helpers.AreCustomersPartners(householdCustomers[0].MaritalStatusId, householdCustomers[1].MaritalStatusId);
    }
}