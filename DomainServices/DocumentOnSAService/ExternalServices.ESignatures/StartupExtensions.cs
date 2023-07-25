using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.StartupExtensions;
using ExternalServices.ESignatures.Abstraction;
using ExternalServices.ESignatures.Configuration;
using ExternalServices.ESignatures.V1;
using ExternalServices.ESignatures.V1.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.ESignatures;

public static class StartupExtensions
{
    internal const string ServiceName = "ESignatures";
    
    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IESignaturesRepository
    {
        ErrorCodeMapper.Init();
     
        var version = GetVersion<TClient>();
        
        var configuration = builder.Configuration.GetSection(GetSectionName(version))
            .Get<ESignaturesConfiguration>();
        
        ValidateConfiguration(configuration, version);
        
        builder.Services.AddSingleton(configuration!);

        builder.Services.AddDapper<IESignaturesDapperConnectionProvider>(configuration!.ConnectionString);
        
        if (version == IESignaturesRepository.Version)
        {
            var implementationType = configuration.ImplementationType == ServiceImplementationTypes.Mock
                ? typeof(MockESignaturesRepository)
                : typeof(RealESignaturesRepository);
            
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
            Type t when t.IsAssignableFrom(typeof(IESignaturesRepository)) => IESignaturesRepository.Version,
            _ => throw new NotImplementedException($"Unknown implementation {typeof(TClient)}")
        };

    private static string GetSectionName(string version)
    {
        return $"{CIS.Core.CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ServiceName}:{version}";
    }

    private static void ValidateConfiguration(ESignaturesConfiguration? configuration, string version)
    {
        if (configuration is null)
            throw new CisConfigurationNotFound(GetSectionName(version));
        if (string.IsNullOrEmpty(configuration.ConnectionString))
            throw new CisConfigurationException(0, $"{ServiceName} {nameof(configuration.ConnectionString)} must be defined");
        if (configuration.ImplementationType == ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{ServiceName} Service client Implementation type is not set");
    }
}