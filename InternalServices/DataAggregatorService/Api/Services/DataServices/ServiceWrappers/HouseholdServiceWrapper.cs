using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

[ScopedService, SelfService]
internal class HouseholdServiceWrapper : IServiceWrapper, IDisposable
{
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;

    private readonly SemaphoreSlim _semaphore = new(1);

    public HouseholdServiceWrapper(IHouseholdServiceClient householdService, ICustomerOnSAServiceClient customerOnSAService)
    {
        _householdService = householdService;
        _customerOnSAService = customerOnSAService;
    }

    public DataService DataService => DataService.HouseholdService;

    public void Dispose()
    {
        _semaphore.Dispose();
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        await LoadHouseholds(input.SalesArrangementId!.Value, data, cancellationToken);
    }

    public async Task LoadMainHouseholdDetail(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        await LoadHouseholds(input.SalesArrangementId!.Value, data, cancellationToken);

        if (data.HouseholdMain is not null)
            await LoadHouseholdDetail(data.HouseholdMain, cancellationToken);
    }

    public async Task LoadCodebtorHouseholdDetail(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        await LoadHouseholds(input.SalesArrangementId!.Value, data, cancellationToken);

        if (data.HouseholdCodebtor is not null)
            await LoadHouseholdDetail(data.HouseholdCodebtor, cancellationToken);
    }

    public async Task LoadAllHouseholdsDetail(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        await LoadHouseholds(input.SalesArrangementId!.Value, data, cancellationToken);

        foreach (var householdInfo in data.Households) 
            await LoadHouseholdDetail(householdInfo, cancellationToken);
    }

    private async Task LoadHouseholds(int salesArrangementId, AggregatedData data, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (data.Households.Any())
                return;

            var households = (await _householdService.GetHouseholdList(salesArrangementId, cancellationToken))
                             .Select(household => new { Household = new HouseholdInfo { Household = household }, HouseholdType = (HouseholdTypes)household.HouseholdTypeId })
                             .ToList();

            data.Households.AddRange(households.Select(h => h.Household));

            data.HouseholdMain = households.FirstOrDefault(h => h.HouseholdType == HouseholdTypes.Main)?.Household;
            data.HouseholdCodebtor = households.FirstOrDefault(h => h.HouseholdType == HouseholdTypes.Codebtor)?.Household;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task LoadHouseholdDetail(HouseholdInfo? householdInfo, CancellationToken cancellationToken)
    {
        if (householdInfo is null)
            return;

        var customer1Loader = Task.FromResult<CustomerOnSA>(null!);
        var customer2Loader = Task.FromResult<CustomerOnSA>(null!);

        if (householdInfo.Household.CustomerOnSAId1.HasValue)
            customer1Loader = _customerOnSAService.GetCustomer(householdInfo.Household.CustomerOnSAId1.Value, cancellationToken);

        if (householdInfo.Household.CustomerOnSAId2.HasValue)
            customer2Loader = _customerOnSAService.GetCustomer(householdInfo.Household.CustomerOnSAId2.Value, cancellationToken);

        householdInfo.CustomerOnSa1 = await customer1Loader;
        householdInfo.CustomerOnSa2 = await customer2Loader;
    }
}