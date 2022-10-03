using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using ExternalServices.Eas;
using ExternalServices.Sulm;

namespace DomainServices.HouseholdService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration?.EAS is null)
            throw new CisConfigurationNotFound("AppConfiguration");
    }

    public static WebApplicationBuilder AddHouseholdService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
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
        // sulm
        builder.AddExternalServiceSulm();

        // dbcontext
        builder.AddEntityFramework<Repositories.HouseholdServiceDbContext>();

        return builder;
    }
}
