using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.Party.V1;
using ExternalServices.Party.V1.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "Party";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
     where TClient : class, IPartyClient
     => builder.AddPartyClient<TClient>(IPartyClient.Version);


    private static WebApplicationBuilder AddPartyClient<TClient>(this WebApplicationBuilder builder, string version)
       where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (IPartyClient.Version, ServiceImplementationTypes.Real):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(RealPartyClient), ServiceLifetime.Scoped));
                break;
            case (IPartyClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(MockPartyClient), ServiceLifetime.Scoped));
                break;
            default:
                throw new NotImplementedException($"{ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
