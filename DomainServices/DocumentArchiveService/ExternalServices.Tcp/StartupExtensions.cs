using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.Configuration;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.Data;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Repositories;
using Google.Rpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp;
public static class StartupExtensions
{
    internal const string ServiceName = "Tcp";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
       where TClient : class, IDocumentServiceRepository
    {
        ErrorCodeMapper.Init();

        string version = getVersion<TClient>();

        var configuration = builder.Configuration.GetSection(GetSectionName(version))
            .Get<TcpConfiguration>();

        ValidateConfiguration(configuration, version);

        builder.Services.AddSingleton(configuration!);

        builder.Services.AddDapperOracle<ITcpDapperConnectionProvider>(configuration!.Connectionstring);

        if (version == IDocumentServiceRepository.Version && configuration.ImplementationType == ServiceImplementationTypes.Mock)
        {
            builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(MockDocumentServiceRepository), ServiceLifetime.Scoped));
            builder.Services.AddHttpClient<ITcpClient, TcpClientMock>();
        }
        else if (version == IDocumentServiceRepository.Version && configuration.ImplementationType == ServiceImplementationTypes.Real)
        {
            builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(RealDocumentServiceRepository), ServiceLifetime.Scoped));
            builder.Services.AddHttpClient<ITcpClient, TcpClient>();
        }
        else
        {
            throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} repository not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
      => typeof(TClient) switch
      {
          Type t when t.IsAssignableFrom(typeof(IDocumentServiceRepository)) => IDocumentServiceRepository.Version,
          _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
      };

    private static string GetSectionName(string version)
    {
        return $"{CIS.Core.CisGlobalConstants.ExternalServicesConfigurationSectionName}:{ServiceName}:{version}";
    }

    private static void ValidateConfiguration(TcpConfiguration? configuration, string version)
    {
        if (configuration is null)
            throw new CisConfigurationNotFound(GetSectionName(version));
        if (string.IsNullOrEmpty(configuration.Connectionstring))
            throw new CisConfigurationException(0, $"{ServiceName} Connectionstring must be defined");
        if (configuration.ImplementationType == ServiceImplementationTypes.Unknown)
            throw new CisConfigurationException(0, $"{ServiceName} Service client Implementation type is not set");
    }
}
