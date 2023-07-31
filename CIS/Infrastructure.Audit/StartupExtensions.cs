using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.Audit;

public static class StartupExtensions
{
    public static WebApplicationBuilder AddCisAudit(this WebApplicationBuilder builder)
    {
        // get configuration from json file
        var configuration = builder.Configuration
            .GetSection(_configurationKey)
            .Get<Configuration.AuditLogConfiguration>();

        if (configuration is not null)
        {
            builder.Services.AddSingleton((serviceProvider) =>
            {
                // get server IP
                var server = serviceProvider.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()!.Addresses;
                var serverIp = addresses.First()[7..^1];

                var cisConfiguration = serviceProvider.GetRequiredService<ICisEnvironmentConfiguration>();

                return new Audit.AuditLoggerHelper(serverIp, cisConfiguration, configuration);
            });
            builder.Services.AddScoped<IAuditLogger, AuditLogger>();
        }

        return builder;
    }

    internal const string _configurationKey = "CisTelemetry:Logging:Audit";
}
