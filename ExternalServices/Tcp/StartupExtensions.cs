using CIS.Core.Exceptions;
using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using CIS.Infrastructure.StartupExtensions;
using ExternalServicesTcp.Configuration;
using ExternalServicesTcp.V1.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ExternalServicesTcp;

public static class StartupExtensions
{
    internal const string ServiceName = "Tcp";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
       where TClient : class, IDocumentServiceRepository
    {
        string version = getVersion<TClient>();

        var configuration = builder.Configuration.GetSection(GetSectionName(version))
            .Get<TcpConfiguration>();

        ValidateConfiguration(configuration,version);

        builder.Services.AddSingleton(configuration!);

        builder.Services.AddDapperOracle<Data.ITcpDapperConnectionProvider>(configuration!.Connectionstring);

        if (version == V1.Repositories.IDocumentServiceRepository.Version && configuration.ImplementationType == ServiceImplementationTypes.Mock)
        {
            builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(V1.Repositories.DocumentServiceRepositoryMock), ServiceLifetime.Scoped));
            builder.Services.AddHttpClient<V1.Clients.ITcpClient, V1.Clients.TcpClientMock>();
        }
        else if (version == V1.Repositories.IDocumentServiceRepository.Version && configuration.ImplementationType == ServiceImplementationTypes.Real)
        {
            builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(V1.Repositories.DocumentServiceRepository), ServiceLifetime.Scoped));
            builder.Services.AddHttpClient<V1.Clients.ITcpClient, V1.Clients.TcpClient>();
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
           Type t when t.IsAssignableFrom(typeof(V1.Repositories.IDocumentServiceRepository)) => V1.Repositories.IDocumentServiceRepository.Version,
           _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
       };

    private static string GetSectionName(string version)
    {
        return $"{Constants.ExternalServicesConfigurationSectionName}:{ServiceName}:{version}";
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
