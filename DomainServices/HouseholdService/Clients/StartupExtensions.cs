using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.HouseholdService.Clients;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:HouseholdService";

    public static IServiceCollection AddDomainService<TClient>(this IServiceCollection services)
        where TClient : IHouseholdServiceClient
    {
        services.AddScoped<IHouseholdServiceClient, Services.HouseholdService>();
        services.AddScoped<ICustomerOnSAServiceClient, Services.CustomerOnSAService>();

        services.AddCisGrpcClientUsingServiceDiscovery<Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        services.AddCisGrpcClientUsingServiceDiscovery<Contracts.v1.HouseholdService.HouseholdServiceClient, Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddHouseholdService<TClient>(this IServiceCollection services, string serviceUrl)
        where TClient : IHouseholdServiceClient
    {
        services.AddScoped<IHouseholdServiceClient, Services.HouseholdService>();
        services.AddScoped<ICustomerOnSAServiceClient, Services.CustomerOnSAService>();

        services.AddCisGrpcClientUsingUrl<Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        services.AddCisGrpcClientUsingUrl<Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        return services;
    }
}
