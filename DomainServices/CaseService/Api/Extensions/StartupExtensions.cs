using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using ExternalServices.Eas;
using ExternalServices.EasSimulationHT;
using ExternalServices.SbWebApi;

namespace DomainServices.CaseService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration?.EAS == null)
            throw new CisConfigurationNotFound("EAS");
    }

    public static WebApplicationBuilder AddCaseService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // EAS svc
        builder.Services.AddExternalServiceEas(appConfiguration.EAS);

        // EAS EasSimulationHT svc
        builder.Services.AddExternalServiceEasSimulationHT(appConfiguration.EasSimulationHT);

        // MpHome svc
        builder.AddExternalServiceSbWebApi();

        // dbcontext
        builder.AddEntityFramework<Repositories.CaseServiceDbContext>();

        return builder;
    }
}
