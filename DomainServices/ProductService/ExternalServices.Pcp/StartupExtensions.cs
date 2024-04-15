using CIS.Infrastructure.ExternalServicesHelpers;
using DomainServices.ProductService.ExternalServices.Pcp.V2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddExternalServiceV2<TClient>(this WebApplicationBuilder builder)
          where TClient : class, DomainServices.ProductService.ExternalServices.Pcp.IPcpClient
          => builder.AddPcpClient<TClient>(DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version2);

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, DomainServices.ProductService.ExternalServices.Pcp.IPcpClient
        => builder.AddPcpClient<TClient>(DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version);

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder, string kbHeaderAppComponent, string kbHeaderAppComponentOriginator)
        where TClient : class, DomainServices.ProductService.ExternalServices.Pcp.IPcpClient
        => builder.AddPcpClient<TClient>(DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version, kbHeaderAppComponent, kbHeaderAppComponentOriginator);

    private static WebApplicationBuilder AddPcpClient<TClient>(this WebApplicationBuilder builder, string version, string? kbHeaderAppComponent = null, string? kbHeaderAppComponentOriginator = null)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<DomainServices.ProductService.ExternalServices.Pcp.IPcpClient, DomainServices.ProductService.ExternalServices.Pcp.V1.MockPcpClient>();
                break;

            case (DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<DomainServices.ProductService.ExternalServices.Pcp.IPcpClient, DomainServices.ProductService.ExternalServices.Pcp.V1.RealPcpClient>()
                    .AddExternalServicesKbHeaders(kbHeaderAppComponent, kbHeaderAppComponentOriginator)
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.ServiceName);
                break;
            case (DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.Version2, ServiceImplementationTypes.Real):
                builder.Services.Add(new ServiceDescriptor(typeof(TClient), typeof(RealSpeedPcpClient), ServiceLifetime.Scoped));
                break;
            default:
                throw new NotImplementedException($"{DomainServices.ProductService.ExternalServices.Pcp.IPcpClient.ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
