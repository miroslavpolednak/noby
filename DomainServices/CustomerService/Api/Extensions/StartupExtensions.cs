using CIS.ExternalServicesHelpers;
using CIS.Infrastructure.StartupExtensions;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Clients;
using ExternalServices.MpHome;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Extensions;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddCustomerService(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        builder.Services.AddDapper(builder.Configuration.GetConnectionString("KonsDb"));

        builder.AddCustomerManagementService();

        // CodebookService
        builder.Services.AddCodebookService();

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return builder;
    }

    public static WebApplicationBuilder AddExternalServiceMpHome(this WebApplicationBuilder builder)
    {
        const string MpHomeName = "MpHome";

        var config = builder.Configuration
                            .GetRequiredSection(Constants.ExternalServicesConfigurationSectionName)
                            .GetRequiredSection(MpHomeName)
                            .Get<MpHomeConfiguration>();

        builder.Services.AddExternalServiceMpHome(config);

        return builder;
    }
}
