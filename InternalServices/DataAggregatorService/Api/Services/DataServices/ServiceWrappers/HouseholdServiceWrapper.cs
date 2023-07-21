using CIS.Foms.Enums;
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

    public DataSource DataSource => DataSource.HouseholdService;

    public void Dispose()
    {
        _semaphore.Dispose();
    }

    public async Task LoadData(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (data.HouseholdMain is not null && data.HouseholdCodebtor is not null)
                return;

            var householdList = (await _householdService.GetHouseholdList(input.SalesArrangementId!.Value, cancellationToken)).ToLookup(h => (HouseholdTypes)h.HouseholdTypeId);

            data.HouseholdMain = new HouseholdInfo { Household = householdList[HouseholdTypes.Main].FirstOrDefault() };
            data.HouseholdCodebtor = new HouseholdInfo { Household = householdList[HouseholdTypes.Codebtor].FirstOrDefault() };
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task LoadMainHouseholdDetail(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();

        await LoadData(input, data, cancellationToken);

        if (data.HouseholdMain!.Household is not null)
            data.HouseholdMain = await LoadHouseholdDetail(data.HouseholdMain.Household, cancellationToken);
    }

    public async Task LoadCodebtorHouseholdDetail(InputParameters input, AggregatedData data, CancellationToken cancellationToken)
    {
        input.ValidateSalesArrangementId();
        
        await LoadData(input, data, cancellationToken);

        if (data.HouseholdCodebtor!.Household is not null)
            data.HouseholdCodebtor = await LoadHouseholdDetail(data.HouseholdCodebtor.Household, cancellationToken);
    }

    private async Task<HouseholdInfo> LoadHouseholdDetail(Household household, CancellationToken cancellationToken)
    {
        var customer1Loader = Task.FromResult<CustomerOnSA>(null!);
        var customer2Loader = Task.FromResult<CustomerOnSA>(null!);

        if (household.CustomerOnSAId1.HasValue)
            customer1Loader = _customerOnSAService.GetCustomer(household.CustomerOnSAId1.Value, cancellationToken);

        if (household.CustomerOnSAId2.HasValue)
            customer2Loader = _customerOnSAService.GetCustomer(household.CustomerOnSAId2.Value, cancellationToken);

        return new HouseholdInfo
        {
            Household = household,
            CustomerOnSa1 = await customer1Loader,
            CustomerOnSa2 = await customer2Loader
        };
    }
}