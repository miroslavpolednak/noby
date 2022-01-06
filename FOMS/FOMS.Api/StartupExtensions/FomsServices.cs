﻿using FluentValidation;

namespace FOMS.Api.StartupExtensions;

internal static class FomsServices
{
    public static WebApplicationBuilder AddFomsServices(this WebApplicationBuilder builder)
    {
        // mediatr
        builder.Services
            .AddMediatR(typeof(IApiAssembly).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(WebApiValidationBehaviour<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(IApiAssembly))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<CIS.Core.Security.ICurrentUserAccessor, Infrastructure.Security.FomsCurrentUserAccessor>();
        
        // doc processor factory
        builder.Services.AddTransient<DocumentProcessing.IDocumentProcessorFactory, DocumentProcessing.DocumentProcessorFactory>();

        return builder;
    }
}
