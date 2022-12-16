using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using CIS.InternalServices.DocumentDataAggregator.EasForms;
using CIS.InternalServices.DocumentDataAggregator.EasForms.FormData;
using Microsoft.Extensions.DependencyInjection;
using DomainServices;

namespace CIS.InternalServices.DocumentDataAggregator;

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