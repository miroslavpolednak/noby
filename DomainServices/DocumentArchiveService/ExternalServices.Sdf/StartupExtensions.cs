using CIS.Foms.Enums;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1.Clients;
using DomainServices.DocumentArchiveService.ExternalServices.Sdf.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CIS.Infrastructure.ExternalServicesHelpers;

namespace DomainServices.DocumentArchiveService.ExternalServices.Sdf;
public static class StartupExtensions
{
    internal const string ServiceName = "Sdf";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
       where TClient : class, ISdfClient
    {
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (ISdfClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(MockSdfClient), ServiceLifetime.Scoped));
                break;

            case (ISdfClient.Version, ServiceImplementationTypes.Real):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(RealSdfClient), ServiceLifetime.Scoped));
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(ISdfClient)) => ISdfClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };
}
