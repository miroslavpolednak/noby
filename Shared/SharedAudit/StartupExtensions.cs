﻿using CIS.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace SharedAudit;

public static class StartupExtensions
{
    private const string _testEnv = "TEST";

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
                   var cisConfiguration = serviceProvider.GetRequiredService<ICisEnvironmentConfiguration>();
                   if (cisConfiguration.EnvironmentName?.ToUpper(CultureInfo.InvariantCulture) == _testEnv)
                       return new AuditLoggerInternalMock();

                   // get server IP
                   var server = serviceProvider.GetRequiredService<IServer>();
                   var addresses = server.Features.Get<IServerAddressesFeature>()!.Addresses;
                   var serverIp = addresses.First()[7..^1];

                   return new AuditLoggerInternal(serverIp, cisConfiguration, configuration);
               });
            builder.Services.AddScoped<IAuditLogger, AuditLogger>();
        }

        return builder;
    }

    internal const string _configurationKey = "CisTelemetry:Logging:Audit";
}