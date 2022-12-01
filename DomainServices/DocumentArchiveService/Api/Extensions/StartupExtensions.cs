﻿using CIS.Infrastructure.StartupExtensions;
using ExternalServices.Sdf;
using ExternalServicesTcp;
using ExternalServicesTcp.Configuration;
using FluentValidation;
using System.Configuration;

namespace DomainServices.DocumentArchiveService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddDocumentArchiveService(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        var appConfig = configuration.GetSection(AppConfiguration.SectionName).Get<AppConfiguration>();

        builder.Services.AddExternalServiceSdf(appConfig.Sdf!);

        builder.Services.AddExternalServiceTcp(appConfig.Tcp!);

        // databases
        builder.Services
            .AddDapper<Data.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("default"));

        return builder;
    }
}