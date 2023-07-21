using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.ServiceWrappers;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;

[SingletonService, SelfService]
internal class ServiceMap
{
    public delegate Task ServiceCall(InputParameters input, AggregatedData data, CancellationToken cancellationToken);
    
    private readonly Dictionary<DataSource, IServiceMapItem> _map = new();

    public ServiceMap()
    {
        ConfigureServices();
    }

    public ServiceCall GetServiceCallFunc(DataSource dataSource, IServiceProvider serviceProvider) => _map[dataSource].GetServiceCall(serviceProvider);

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
    }

    private void AddService<TService>(DataSource dataSource) where TService : IServiceWrapper
    {
        var mapItem = new ServiceMapItem<TService>
        {
            ServiceCallFactory = CommonServiceCall
        };

        _map.Add(dataSource, mapItem);

        static ServiceCall CommonServiceCall(TService service) => service.LoadData;
    }

    private void AddService<TService>(DataSource dataSource, Func<TService, ServiceCall> serviceCallFactory) where TService : IServiceWrapper
    {
        var mapItem = new ServiceMapItem<TService>
        {
            ServiceCallFactory = serviceCallFactory
        };

        _map.Add(dataSource, mapItem);
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