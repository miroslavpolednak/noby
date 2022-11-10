using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.DocumentDataAggregator.Configuration.Data;
using DomainServices.CaseService.Abstraction;
using DomainServices.CodebookService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.UserService.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DocumentDataAggregator;

public static class StartupExtensions
{
    public static IServiceCollection AddDataAggregator(this IServiceCollection services)
    {
        services.AddDbContext<ConfigurationContext>(ServiceLifetime.Transient);

        services.AddTransient<IDataAggregator, DataAggregator>();

        services.AddAttributedServices(typeof(StartupExtensions));

        services.AddDomainServices();

        return services;
    }

    private static void AddDomainServices(this IServiceCollection services) =>
        services.AddCodebookService()
                .AddSalesArrangementService()
                .AddCaseService()
                .AddOfferService()
                .AddUserService();
}