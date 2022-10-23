using CIS.Infrastructure.StartupExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DocumentDataAggregator;

public static class StartupExtensions
{
    public static IServiceCollection AddDataAggregator(this IServiceCollection services)
    {
        services.AddScoped<IDataAggregator, DataAggregator>();

        services.AddAttributedServices(typeof(StartupExtensions));

        return services;
    }
}