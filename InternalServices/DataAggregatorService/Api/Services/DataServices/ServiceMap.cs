using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

[SingletonService, SelfService]
internal class ServiceMap
{
    public delegate Task ServiceCall(InputParameters input, AggregatedData data, CancellationToken cancellationToken);
    
    private readonly Dictionary<DataService, IServiceMapItem> _map = new();

    public ServiceMap()
    {
        ConfigureServices();
    }

    public ServiceCall GetServiceCallFunc(DataService dataService, IServiceProvider serviceProvider) => _map[dataService].GetServiceCall(serviceProvider);

    private void ConfigureServices()
    {
        AddService<SalesArrangementServiceWrapper>(DataService.SalesArrangementService);
        AddService<CaseServiceWrapper>(DataService.CaseService);
        AddService<OfferServiceWrapper>(DataService.OfferService);
        AddService<UserServiceWrapper>(DataService.UserService);
        AddService<CustomerServiceWrapper>(DataService.CustomerService);
        AddService<ProductServiceWrapper>(DataService.ProductService);
        AddService<HouseholdServiceWrapper>(DataService.HouseholdService);
        AddService<DocumentOnSaServiceWrapper>(DataService.DocumentOnSa);

        ConfigureExtensionServices();
    }

    private void ConfigureExtensionServices()
    {
        AddService<OfferServiceWrapper>(DataService.OfferPaymentScheduleService, s => s.LoadPaymentSchedule);
        AddService<HouseholdServiceWrapper>(DataService.HouseholdMainService, s => s.LoadMainHouseholdDetail);
        AddService<HouseholdServiceWrapper>(DataService.HouseholdCodebtorService, s => s.LoadCodebtorHouseholdDetail);
        AddService<HouseholdServiceWrapper>(DataService.HouseholdAllService, s => s.LoadAllHouseholdsDetail);
    }

    private void AddService<TService>(DataService dataService) where TService : IServiceWrapper
    {
        var mapItem = new ServiceMapItem<TService>
        {
            ServiceCallFactory = CommonServiceCall
        };

        _map.Add(dataService, mapItem);

        static ServiceCall CommonServiceCall(TService service) => service.LoadData;
    }

    private void AddService<TService>(DataService dataService, Func<TService, ServiceCall> serviceCallFactory) where TService : IServiceWrapper
    {
        var mapItem = new ServiceMapItem<TService>
        {
            ServiceCallFactory = serviceCallFactory
        };

        _map.Add(dataService, mapItem);
    }

    private interface IServiceMapItem
    {
        ServiceCall GetServiceCall(IServiceProvider serviceProvider);
    }

    private record ServiceMapItem<TService> : IServiceMapItem where TService : IServiceWrapper
    {
        public required Func<TService, ServiceCall> ServiceCallFactory { get; init; }

        public ServiceCall GetServiceCall(IServiceProvider serviceProvider) => ServiceCallFactory(serviceProvider.GetRequiredService<TService>());
    }
}