using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using System.Text;

namespace ExternalServices.MpHome;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceEas(this IServiceCollection services, MpHomeConfiguration? mpHomeConfiguration)
    {
        if (mpHomeConfiguration == null)
            throw new ArgumentNullException(nameof(mpHomeConfiguration), "MpHome configuration not set");
        if (!mpHomeConfiguration.UseServiceDiscovery && string.IsNullOrEmpty(mpHomeConfiguration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "MpHome Service URL must be defined");
        if (mpHomeConfiguration.ImplementationType == CIS.Core.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "MpHome Service client Implementation type is not set");

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (mpHomeConfiguration.UseServiceDiscovery)
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("ES:MpHome"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                mpHomeConfiguration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:MpHome Proprietary service URL");
            }
            return mpHomeConfiguration;
        });

        switch (mpHomeConfiguration.Version)
        {
            case Versions.V1:
                if (mpHomeConfiguration.ImplementationType == CIS.Core.ServiceImplementationTypes.Mock)
                    services.AddScoped<V1.IMpHomeClient, V1.MockMpHomeClient>();
                else
                    services.AddHttpClient<V1.IMpHomeClient, V1.RealMpHomeClient>(c =>
                    {
                        c.BaseAddress = new Uri(mpHomeConfiguration.ServiceUrl);
                        var byteArray = Encoding.ASCII.GetBytes($"{mpHomeConfiguration.Username}:{mpHomeConfiguration.Password}");
                        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    });
                break;

            default:
                throw new NotImplementedException($"MpHome version {mpHomeConfiguration.Version} client not implemented");
        }

        return services;
    }
}
