using CIS.Foms.Enums;
using CIS.Infrastructure.ExternalServicesHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices;

public static class StartupExtensions
{
    internal const string ServiceName = "Pcp";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient
        => builder.AddPcpClient<TClient>(DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient.Version);

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder, string kbHeaderAppComponent, string kbHeaderAppComponentOriginator)
        where TClient : class, DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient
        => builder.AddPcpClient<TClient>(DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient.Version, kbHeaderAppComponent, kbHeaderAppComponentOriginator);

    private static WebApplicationBuilder AddPcpClient<TClient>(this WebApplicationBuilder builder, string version, string? kbHeaderAppComponent = null, string? kbHeaderAppComponentOriginator = null)
        where TClient : class, IExternalServiceClient
    {
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient, DomainServices.ProductService.ExternalServices.Pcp.V1.MockPcpClient>();
                break;

            case (DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<DomainServices.ProductService.ExternalServices.Pcp.V1.IPcpClient, DomainServices.ProductService.ExternalServices.Pcp.V1.RealPcpClient>()
                    .AddExternalServicesKbHeaders(kbHeaderAppComponent, kbHeaderAppComponentOriginator)
                    .AddExternalServicesKbPartyHeaders();
                    //.AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {version} client not implemented");
        }

        return builder;
    }
}
