using Microsoft.Extensions.DependencyInjection;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using System.Text;

namespace ExternalServices.ESignatures;

public static class StartupExtensions
{
    public static IServiceCollection AddExternalServiceESignatures(this IServiceCollection services, ESignaturesConfiguration? eSignaturesConfiguration)
    {
        if (eSignaturesConfiguration == null)
            throw new ArgumentNullException(nameof(eSignaturesConfiguration), "ESignatures configuration not set");
        if (!eSignaturesConfiguration.UseServiceDiscovery && string.IsNullOrEmpty(eSignaturesConfiguration.ServiceUrl))
            throw new ArgumentNullException("ServiceUrl", "ESignatures Service URL must be defined");
        if (eSignaturesConfiguration.ImplementationType == CIS.Core.ServiceImplementationTypes.Unknown)
            throw new ArgumentException("ImplementationType", "ESignatures Service client Implementation type is not set");

        services.AddSingleton(provider =>
        {
            // pokud se ma hledat URL v service discovery
            if (eSignaturesConfiguration.UseServiceDiscovery)
            {
                string? url = provider.GetRequiredService<IDiscoveryServiceAbstraction>()
                    .GetService(new("ES:ESignatures"), CIS.InternalServices.ServiceDiscovery.Contracts.ServiceTypes.Proprietary)
                    .GetAwaiter()
                    .GetResult()?
                    .ServiceUrl;
                eSignaturesConfiguration.ServiceUrl = url ?? throw new ArgumentNullException("url", "Service Discovery can not find ES:ESignatures Proprietary service URL");
            }
            return eSignaturesConfiguration;
        });

        switch (eSignaturesConfiguration.Version)
        {
            case Versions.V1:
                if (eSignaturesConfiguration.ImplementationType == CIS.Core.ServiceImplementationTypes.Mock)
                    services.AddScoped<V1.IESignaturesClient, V1.MockESignaturesClient>();
                else
                    services.AddHttpClient<V1.IESignaturesClient, V1.RealESignaturesClient>(c =>
                    {
                        c.BaseAddress = new Uri(eSignaturesConfiguration.ServiceUrl);
                        var byteArray = Encoding.ASCII.GetBytes($"{eSignaturesConfiguration.Username}:{eSignaturesConfiguration.Password}");
                        c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    });
                break;

            default:
                throw new NotImplementedException($"ESignatures version {eSignaturesConfiguration.Version} client not implemented");
        }

        return services;
    }
}
