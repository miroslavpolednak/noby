using CIS.Infrastructure.gRPC;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.HouseholdService.Clients;
using __Services = DomainServices.HouseholdService.Clients.Services;
using __Contracts = DomainServices.HouseholdService.Contracts;

namespace DomainServices;

public static class StartupExtensions
{
    /// <summary>
    /// Service SD key
    /// </summary>
    public const string ServiceName = "DS:HouseholdService";

    public static IServiceCollection AddHouseholdService(this IServiceCollection services)
    {
        services.AddScoped<IHouseholdServiceClient, __Services.HouseholdService>();
        services.AddScoped<ICustomerOnSAServiceClient, __Services.CustomerOnSAService>();

        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        services.AddCisGrpcClientUsingServiceDiscovery<__Contracts.v1.HouseholdService.HouseholdServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(ServiceName);
        return services;
    }

    public static IServiceCollection AddHouseholdService(this IServiceCollection services, string serviceUrl)
    {
        services.AddScoped<IHouseholdServiceClient, __Services.HouseholdService>();
        services.AddScoped<ICustomerOnSAServiceClient, __Services.CustomerOnSAService>();

        services.AddCisGrpcClientUsingUrl<__Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        services.AddCisGrpcClientUsingUrl<__Contracts.v1.CustomerOnSAService.CustomerOnSAServiceClient, __Contracts.v1.HouseholdService.HouseholdServiceClient>(serviceUrl);
        return services;
    }
}
