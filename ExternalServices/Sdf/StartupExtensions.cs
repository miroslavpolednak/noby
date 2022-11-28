using ExternalServices.Sdf.Configuration;
using ExternalServices.Sdf.V1.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices.Sdf
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddExternalServiceSdf(this IServiceCollection services, SdfConfiguration sdfConfiguration)
        {
            if (sdfConfiguration is null)
            {
                throw new ArgumentNullException(nameof(sdfConfiguration));
            }

            services.AddSingleton(sdfConfiguration);
            
            services.AddScoped<ISdfClient, SdfClient>();

            return services;
        }
    }
}
