using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using System.Text;

namespace ExternalServices.Rip;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceRip(this IServiceCollection services, RipConfiguration? ripConfiguration)
    {
        if (ripConfiguration == null)
            throw new ArgumentNullException(nameof(ripConfiguration), "Rip configuration not set");
        if (!ripConfiguration.UseServiceDiscovery && string.IsNullOrEmpty(ripConfiguration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "Rip Service URL must be defined");
        if (ripConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "Rip Service client Implementation type is not set");

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (ripConfiguration.UseServiceDiscovery)
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("ES:Rip"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                ripConfiguration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:Rip Proprietary service URL");
            }
            return ripConfiguration;
        });

        switch (ripConfiguration.Version)
        {
            case Versions.V1:
                if (ripConfiguration.ImplementationType == CIS.Foms.Enums.ServiceImplementationTypes.Mock)
                    services.AddScoped<V1.IRipClient, V1.MockRipClient>();
                else
                    services.AddHttpClient<V1.IRipClient, V1.RealRipClient>(c =>
                    {
                        c.BaseAddress = new Uri(ripConfiguration.ServiceUrl);
                        var byteArray = Encoding.ASCII.GetBytes($"{ripConfiguration.Username}:{ripConfiguration.Password}");
                        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    });
                break;

            default:
                throw new NotImplementedException($"Rip version {ripConfiguration.Version} client not implemented");
        }

        return services;
    }
}
