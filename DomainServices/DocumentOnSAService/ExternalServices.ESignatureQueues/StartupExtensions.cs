using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.StartupExtensions;
using ExternalServices.ESignatureQueues.Abstraction;
using ExternalServices.ESignatureQueues.Configuration;
using ExternalServices.ESignatureQueues.V1;
using ExternalServices.ESignatureQueues.V1.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.ESignatureQueues;

public static class StartupExtensions
{
    internal const string ServiceName = "ESignatureQueues";
    
    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IESignatureQueuesRepository
    {
        ErrorCodeMapper.Init();
     
        var version = GetVersion<TClient>();
        
        var configuration = builder.Configuration.GetSection(GetSectionName(version))
            .Get<ESignatureQueuesConfiguration>();
        
        ValidateConfiguration(configuration, version);
        
        builder.Services.AddSingleton(configuration!);

        builder.Services.AddDapper<IESignatureQueuesDapperConnectionProvider>(configuration!.ConnectionString);
        
        if (version == IESignatureQueuesRepository.Version)
        {
            var implementationType = configuration.ImplementationType == ServiceImplementationTypes.Mock
                ? typeof(MockESignatureQueuesRepository)
                : typeof(RealESignatureQueuesRepository);
            
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
            Type t when t.IsAssignableFrom(typeof(IESignatureQueuesRepository)) => IESignatureQueuesRepository.Version,
            _ => throw new NotImplementedException($"Unknown implementation {typeof(TClient)}")
        };

    private static string GetSectionName(string version)
    {
        return $"{CIS.Core.CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ServiceName}:{version}";
    }

    private static void ValidateConfiguration(ESignatureQueuesConfiguration? configuration, string version)
    {
        if (configuration is null)
            throw new CisConfigurationNotFound(GetSectionName(version));
        if (string.IsNullOrEmpty(configuration.ConnectionString))
            throw new CisConfigurationException(0, $"{ServiceName} {nameof(configuration.ConnectionString)} must be defined");
        if (configuration.ImplementationType == ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{ServiceName} Service client Implementation type is not set");
    }
}