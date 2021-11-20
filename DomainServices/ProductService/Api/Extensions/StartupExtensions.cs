using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Abstraction;
using ExternalServices.Eas;

namespace DomainServices.ProductService.Api;

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
        if (configuration.MpHome == null)
            throw new ArgumentNullException("AppConfiguration.MPHome");
    }

    public static IServiceCollection AddProductService(this IServiceCollection services, AppConfiguration appConfiguration, IConfiguration configuration)
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
        services.AddDapper<Repositories.NobyDbRepository>(configuration.GetConnectionString("noby"));
        services.AddDapper<Repositories.KonsDbRepository>(configuration.GetConnectionString("konsdb"));

        // repos
        services.AddScoped<Repositories.KonsDbRepository>();
        services.AddScoped<Repositories.NobyDbRepository>();

        services.AddHttpContextAccessor();
        services.AddCisCurrentUser();
        services.AddCodebookService(true);
            
        return services;
    }

    private static IServiceCollection registerMpHome(CIS.Core.ServiceImplementationTypes? implementation, IServiceCollection services)
        => implementation switch
        {
            CIS.Core.ServiceImplementationTypes.Mock => services.AddScoped<MpHome.IMpHomeClient, MpHome.MockMpHomeClient>(),
            CIS.Core.ServiceImplementationTypes.Real => services.AddScoped<MpHome.IMpHomeClient, MpHome.RealMpHomeClient>(),
            _ => throw new NotImplementedException("MpHome Implementation type is not set")
        };

}
