using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DataAggregator.Configuration.Data;
using CIS.InternalServices.DataAggregator.EasForms;
using CIS.InternalServices.DataAggregator.EasForms.FormData;
using DomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator;

public static class StartupExtensions
{
    public static IServiceCollection AddDataAggregator(this IServiceCollection services)
    {
        services.AddDbContext<ConfigurationContext>(ServiceLifetime.Transient);

        services.AddTransient<IDataAggregator, DataAggregator>()
                .AddTransient<IServiceFormData, ServiceFormData>()
                .AddTransient<IProductFormData, ProductFormData>();

        services.AddAttributedServices(typeof(StartupExtensions));

        services.AddDomainServices();

        return services;
    }

    private static void AddDomainServices(this IServiceCollection services) =>
        services.AddCodebookService()
                .AddSalesArrangementService()
                .AddCaseService()
                .AddOfferService()
                .AddUserService()
                .AddCustomerService()
                .AddProductService()
                .AddHouseholdService();
}