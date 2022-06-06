using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ExternalServices.Eas;
using ExternalServices.Rip;
using ExternalServices.SbWebApi;

namespace DomainServices.SalesArrangementService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration?.EAS is null)
            throw new CisConfigurationNotFound("AppConfiguration");
        if (configuration?.Rip == null)
            throw new ArgumentNullException("AppConfiguration.Rip", nameof(configuration.Rip));
    }

    public static WebApplicationBuilder AddSalesArrangementService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
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

        // Rip svc
        builder.Services.AddExternalServiceRip(appConfiguration.Rip);
        // sb web api
        builder.AddExternalServiceSbWebApi();

        // dbcontext
        builder.AddEntityFramework<Repositories.SalesArrangementServiceDbContext>();
        builder.AddEntityFramework<Repositories.NobyDbContext>("nobyDb");

        return builder;
    }
}
