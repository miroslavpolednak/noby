using CIS.Core.Exceptions;
using CIS.Infrastructure.StartupExtensions;
using ExternalServices.SbQueues.Abstraction;
using ExternalServices.SbQueues.Configuration;
using ExternalServices.SbQueues.V1;
using ExternalServices.SbQueues.V1.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace ExternalServices.SbQueues;

public static class StartupExtensions
{
    internal const string ServiceName = "SbQueues";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, ISbQueuesRepository
    {
        ErrorCodeMapper.Init();

        var version = GetVersion<TClient>();

        var configuration = builder.Configuration.GetSection(GetSectionName(version))
            .Get<SbQueuesConfiguration>();

        var connectionString = builder.Configuration.GetConnectionString("default");

        ValidateConfiguration(configuration, connectionString, version);

        builder.Services.AddSingleton(configuration!);

        builder.Services.AddDapper<ISbQueuesDapperConnectionProvider>(connectionString!);

        if (version == ISbQueuesRepository.Version)
        {
            var implementationType = configuration!.ImplementationType == ServiceImplementationTypes.Mock
                ? typeof(MockSbQueuesRepository)
                : typeof(RealSbQueuesRepository);

            builder.Services.Add(new ServiceDescriptor(typeof(TClient), implementationType, ServiceLifetime.Scoped));
        }
        else
        {
            throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} repository not implemented");
        }

        return builder;
    }

    static string GetVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(ISbQueuesRepository)) => ISbQueuesRepository.Version,
            _ => throw new NotImplementedException($"Unknown implementation {typeof(TClient)}")
        };

    private static string GetSectionName(string version)
    {
        return $"{CIS.Core.CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ServiceName}:{version}";
    }

    private static void ValidateConfiguration(SbQueuesConfiguration? configuration, string? connectionString, string version)
    {
        if (configuration is null)
            throw new CisConfigurationNotFound(GetSectionName(version));
        if (configuration.ImplementationType == ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{ServiceName} Service client Implementation type is not set");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new CisConfigurationException(0, $"{ServiceName} connectionString must be defined");

    }
}