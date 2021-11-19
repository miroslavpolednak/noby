using CIS.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Infrastructure.StartupExtensions
{
    public static class CisEnvironmentConfiguration
    {
        public static IServiceCollection AddCisEnvironmentConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            Configuration.CisEnvironmentConfiguration cisConfiguration = new();
            configuration.GetSection(JsonConfigurationKey).Bind(cisConfiguration);

            CheckAndRegisterConfiguration(services, cisConfiguration);

            return services;
        }

        public static IServiceCollection AddCisEnvironmentConfiguration(this IServiceCollection services, Action<ICisEnvironmentConfiguration> options)
        {
            var cisConfiguration = new Configuration.CisEnvironmentConfiguration();
            options.Invoke(cisConfiguration);

            CheckAndRegisterConfiguration(services, cisConfiguration);

            return services;
        }

        public static IServiceCollection AddCisEnvironmentConfiguration(this IServiceCollection services, IConfiguration configuration, Action<ICisEnvironmentConfiguration> options)
        {
            var cisConfiguration = new Configuration.CisEnvironmentConfiguration();
            configuration.GetSection(JsonConfigurationKey).Bind(cisConfiguration);

            options.Invoke(cisConfiguration);

            CheckAndRegisterConfiguration(services, cisConfiguration);

            return services;
        }

        private const string JsonConfigurationKey = "CisEnvironmentConfiguration";

        private static void CheckAndRegisterConfiguration(IServiceCollection services, ICisEnvironmentConfiguration cisConfiguration)
        {
            if (string.IsNullOrEmpty(cisConfiguration.DefaultApplicationKey))
                throw new ArgumentNullException("Application Key is empty, cannot initialize CIS Environment Configuration", "ApplicationKey");
            if (string.IsNullOrEmpty(cisConfiguration.EnvironmentName))
                throw new ArgumentNullException("Environment Name is empty, cannot initialize CIS Environment Configuration", "EnvironmentName");
            /*if (string.IsNullOrEmpty(cisConfiguration.ServiceDiscoveryUrl))
                throw new ArgumentNullException("Service Discovery Url is empty, cannot initialize CIS Environment Configuration", "ServiceDiscoveryUrl");*/

            services.TryAddSingleton<ICisEnvironmentConfiguration>(cisConfiguration);
        }
    }
}
