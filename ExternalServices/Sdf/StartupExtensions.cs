using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.Sdf.V1.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.Sdf;

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
            case (V1.Clients.ISdfClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(SdfClientMock), ServiceLifetime.Scoped));
                break;

            case (V1.Clients.ISdfClient.Version, ServiceImplementationTypes.Real):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(SdfClient), ServiceLifetime.Scoped));
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
          => typeof(TClient) switch
          {
              Type t when t.IsAssignableFrom(typeof(V1.Clients.ISdfClient)) => V1.Clients.ISdfClient.Version,
              _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
          };
}
