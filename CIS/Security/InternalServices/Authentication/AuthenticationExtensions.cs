using CIS.Security.InternalServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CIS.Security
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Prida autentizaci volajici aplikace do aktualni sluzby - pouziva se pouze pro interni sluzby. Autentizace je technickym uzivatelem.
        /// Konfigurace autentizacniho middleware je v CisSecurity:ServiceAuthentication.
        /// </summary>
        public static IServiceCollection AddCisServiceAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // get configuration
            var c = new Configuration.CisServiceAuthenticationConfiguration();
            configuration.GetSection("CisSecurity:ServiceAuthentication").Bind(c);
            if (c == null || (string.IsNullOrEmpty(c.AdHost) && c.Validator == Configuration.CisServiceAuthenticationConfiguration.LoginValidators.ActiveDirectory))
                throw new System.ArgumentNullException("CisSecurity:ServiceAuthentication not configured in appsettings.json");

            // header parser
            services.TryAddSingleton<IAuthHeaderParser, AuthHeaderParser>();

            // login validator
            switch (c.Validator)
            {
                case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.StaticCollection:
                    services.TryAddSingleton<ILoginValidator, StaticLoginValidator>();
                    break;
                case Configuration.CisServiceAuthenticationConfiguration.LoginValidators.ActiveDirectory:
                    services.TryAddSingleton<ILoginValidator, AdLoginValidator>();
                    break;
                default:
                    throw new System.Exception($"Unknown LoginValidator {c.Validator}");
            }
            
            services
                .AddAuthentication(InternalServicesAuthentication.DefaultSchemeName)
                .AddScheme<CisServiceAuthenticationOptions, CisServiceAuthenticationHandler>(InternalServicesAuthentication.DefaultSchemeName, options =>
                {
                    options.DomainUsernamePrefix = c.DomainUsernamePrefix;
                    options.AdHost = c.AdHost;
                    options.AdPort = c.AdPort ?? 0;
                });

            services.AddAuthorization();

            return services;
        }
    }
}
