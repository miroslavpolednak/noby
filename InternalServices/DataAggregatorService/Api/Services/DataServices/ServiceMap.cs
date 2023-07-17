using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

[SingletonService, SelfService]
internal class ServiceMap
{
    public delegate Task ServiceCall(InputParameters input, AggregatedData data, CancellationToken cancellationToken);

    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<DataSource, IServiceMapItem> _map = new();

    public ServiceMap(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider.CreateScope().ServiceProvider;

        ConfigureServices();
    }

    public ServiceCall GetServiceCallFunc(DataSource dataSource) => _map[dataSource].GetServiceCall();

    private void ConfigureServices()
    {
        AddService<SalesArrangementServiceWrapper>(DataSource.SalesArrangementService);
        AddService<CaseServiceWrapper>(DataSource.CaseService);
        AddService<OfferServiceWrapper>(DataSource.OfferService);
        AddService<UserServiceWrapper>(DataSource.UserService);
        AddService<CustomerServiceWrapper>(DataSource.CustomerService);
        AddService<ProductServiceWrapper>(DataSource.ProductService);
        AddService<HouseholdServiceWrapper>(DataSource.HouseholdService);
        AddService<DocumentOnSaServiceWrapper>(DataSource.DocumentOnSa);

        ConfigureExtensionServices();
    }

    private void ConfigureExtensionServices()
    {
        AddService<OfferServiceWrapper>(DataSource.OfferPaymentScheduleService, s => s.LoadPaymentSchedule);
        AddService<HouseholdServiceWrapper>(DataSource.HouseholdMainService, s => s.LoadMainHouseholdDetail);
        AddService<HouseholdServiceWrapper>(DataSource.HouseholdCodebtorService, s => s.LoadCodebtorHouseholdDetail);
        AddService<HouseholdServiceWrapper>(DataSource.HouseholdAllService, s => s.LoadAllHouseholdsDetail);
    }

    private void AddService<TService>(DataSource dataSource) where TService : IServiceWrapper
    {
        var mapItem = new ServiceMapItem<TService>
        {
            ServiceFactory = _serviceProvider.GetRequiredService<TService>,
            ServiceCallFactory = CommonServiceCall
        };

        _map.Add(dataSource, mapItem);

        static ServiceCall CommonServiceCall(TService service) => service.LoadData;
    }

    private void AddService<TService>(DataSource dataSource, Func<TService, ServiceCall> serviceCallFactory) where TService : IServiceWrapper
    {
        var mapItem = new ServiceMapItem<TService>
        {
            ServiceFactory = _serviceProvider.GetRequiredService<TService>,
            ServiceCallFactory = serviceCallFactory
        };

        _map.Add(dataSource, mapItem);
    }

    private interface IServiceMapItem
    {
        ServiceCall GetServiceCall();
    }

    private record ServiceMapItem<TService> : IServiceMapItem where TService : IServiceWrapper
    {
        public required Func<TService> ServiceFactory { get; init; }

        public required Func<TService, ServiceCall> ServiceCallFactory { get; init; }

        public ServiceCall GetServiceCall() => ServiceCallFactory(ServiceFactory());
    }
}