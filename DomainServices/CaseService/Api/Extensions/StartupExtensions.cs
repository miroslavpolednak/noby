using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Abstraction;
using ExternalServices.Eas;

namespace DomainServices.CaseService.Api;

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

    public static IServiceCollection AddCaseService(this IServiceCollection services, AppConfiguration appConfiguration, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // add validators
        services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // EAS svc
        services.AddExternalServiceEas(appConfiguration.EAS);
        // MpHome svc
        //registerMpHome(appConfiguration.EAS?.Implementation, services);

        // dbcontext
        string connectionString = configuration.GetConnectionString("default");
        services.AddDbContext<Repositories.CaseServiceDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        services.AddHttpContextAccessor();
        services.AddCisCurrentUser();
            
        return services;
    }
}
