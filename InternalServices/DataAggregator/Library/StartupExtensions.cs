using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DataAggregator;
using CIS.InternalServices.DataAggregator.Configuration.Data;
using CIS.InternalServices.DataAggregator.EasForms;
using CIS.InternalServices.DataAggregator.EasForms.FormData;
using DomainServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using __DS = DomainServices;

namespace CIS.InternalServices;

public static class StartupExtensions
{
    public static IServiceCollection AddDataAggregator(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ConfigurationContext>(opts => opts.UseSqlServer(connectionString),
                                                    ServiceLifetime.Transient);

        services.AddTransient<IDataAggregator, DataAggregator.DataAggregator>()
                .AddTransient<IServiceFormData, ServiceFormData>()
                .AddTransient<IProductFormData, ProductFormData>();

        services.AddAttributedServices(typeof(StartupExtensions));

        services.AddDomainServices();

        return services;
    }

    private static void AddDomainServices(this IServiceCollection services)
    {
        TryAddService<__DS.CodebookService.Clients.ICodebookServiceClients>(services.AddCodebookService);
        TryAddService<__DS.SalesArrangementService.Clients.ISalesArrangementServiceClient>(services.AddSalesArrangementService);
        TryAddService<__DS.CaseService.Clients.ICaseServiceClient>(services.AddCaseService);
        TryAddService<__DS.OfferService.Clients.IOfferServiceClient>(services.AddOfferService);
        TryAddService<__DS.UserService.Clients.IUserServiceClient>(services.AddUserService);
        TryAddService<__DS.CustomerService.Clients.ICustomerServiceClient>(services.AddCustomerService);
        TryAddService<__DS.ProductService.Clients.IProductServiceClient>(services.AddProductService);
        TryAddService<__DS.HouseholdService.Clients.IHouseholdServiceClient>(services.AddHouseholdService);

        void TryAddService<TServiceType>(Func<IServiceCollection> func)
        {
            if (services.Any(x => x.ServiceType != typeof(TServiceType)))
                return;

            func();
        }
    }
}