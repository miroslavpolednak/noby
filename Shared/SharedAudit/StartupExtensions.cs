using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedAudit;

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
public static class AuditLoggingStartupExtensions
{
    public static WebApplicationBuilder AddCisAudit(this WebApplicationBuilder builder)
    {
        // get configuration from json file
        var configuration = builder.Configuration
            .GetSection(_configurationKey)
            .Get<Configuration.AuditLogConfiguration>();

        if (configuration is not null)
        {
            if (string.IsNullOrEmpty(configuration.HashSecretKey))
            {
                throw new CIS.Core.Exceptions.CisConfigurationException(0, "HashSecretKey is not set for audit logging");
            }

            builder.Services.AddSingleton<IAuditLoggerInternal>((serviceProvider) =>
               {
                   // get server IP
                   var server = serviceProvider.GetRequiredService<IServer>();
                   var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;

                   if (addresses?.Any() ?? false)
                   {
                       var serverIp = addresses.First()[7..^1];
                       var cisConfiguration = serviceProvider.GetRequiredService<ICisEnvironmentConfiguration>();

                       return new AuditLoggerInternal(serverIp, cisConfiguration, configuration);
                   }
                   else
                   {
                       return new AuditLoggerInternalMock();
                   }
               });

            builder.Services.AddScoped<IAuditLogger, AuditLogger>();
        }

        return builder;
    }

    public static IManualAuditLogger CreateManualAuditLogger(
        string serverIp,
        string environmentName,
        string applicationKey,
        string eamApplication,
        string eamVersion,
        string hashSecretKey,
        string databaseConnectionString)
    {
        var logger = new AuditLoggerInternal(serverIp, environmentName, applicationKey, eamApplication, eamVersion, hashSecretKey, databaseConnectionString);
        return new ManualAuditLogger(logger);
    }

    internal const string _configurationKey = "CisTelemetry:Logging:Audit";
}
