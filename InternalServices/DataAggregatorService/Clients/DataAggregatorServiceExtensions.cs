using CIS.Infrastructure.gRPC;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Clients.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.InternalServices;

public static class DataAggregatorServiceExtensions
{
    public const string ServiceName = "CIS:DataAggregatorService";

    public static IServiceCollection AddDataAggregatorService(this IServiceCollection services)
    {
        services.AddCisServiceDiscovery();
        services.TryAddTransient<IDataAggregatorServiceClient,  DataAggregatorServiceClient>();
        services.TryAddCisGrpcClientUsingServiceDiscovery<DataAggregatorService.Contracts.V1.DataAggregatorService.DataAggregatorServiceClient>(ServiceName);

        return services;
    }

    public static IServiceCollection AddCustomerService(this IServiceCollection services, string serviceUrl)
    {
        services.TryAddTransient<IDataAggregatorServiceClient, DataAggregatorServiceClient>();
        services.TryAddCisGrpcClientUsingUrl<DataAggregatorService.Contracts.V1.DataAggregatorService.DataAggregatorServiceClient>(serviceUrl);

        return services;
    }
}