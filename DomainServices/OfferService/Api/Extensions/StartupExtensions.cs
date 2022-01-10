using CIS.Infrastructure.StartupExtensions;
using ExternalServices.Eas;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api;

internal static class StartupExtensions
{
    /// <summary>
    /// Kontrola zda je vse v konfiguracnim souboru korektne
    /// </summary>
    public static void CheckAppConfiguration(this AppConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException("AppConfiguration");
        if (configuration.EAS == null)
            throw new ArgumentNullException("AppConfiguration.EAS");
    }

    public static WebApplicationBuilder AddOfferService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
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

        // dbcontext
        string connectionString = builder.Configuration.GetConnectionString("default");
        builder.Services.AddDbContext<Repositories.OfferServiceDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        builder.Services.AddHttpContextAccessor();
        builder.AddCisCurrentUser();
            
        return builder;
    }
}
