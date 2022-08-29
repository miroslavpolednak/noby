using CIS.Infrastructure.StartupExtensions;
using ExternalServices.MpHome;
using ExternalServices.Eas;
using ExternalServices.CustomerManagement;
using FluentValidation;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CustomerService.Api.Clients;

namespace DomainServices.CustomerService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException("AppConfiguration");
    }

    public static WebApplicationBuilder AddCutomerService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // add repos helpers
        builder.Services.AddDapper<Repositories.KonsDbRepository>(builder.Configuration.GetConnectionString("KonsDb"));

        // MpHome
        builder.Services.AddExternalServiceMpHome(appConfiguration.MpHome);

        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);

        builder.AddCustomerManagementService().AddCustomerProfileService();

        // CustomerManagement
        builder.Services.AddExternalServiceCustomerManagement(appConfiguration.CustomerManagement);

        // CodebookService
        builder.Services.AddCodebookService();

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(IApiAssembly))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return builder;
    }
}
