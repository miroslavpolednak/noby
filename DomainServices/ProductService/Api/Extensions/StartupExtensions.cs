using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DomainServices.CodebookService.Abstraction;
using ExternalServices.Eas;
using ExternalServices.MpHome;
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

    public static WebApplicationBuilder AddProductService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
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
        // MpHome svc
        builder.Services.AddExternalServiceMpHome(appConfiguration.MpHome);

        // dbcontext
        builder.AddEntityFramework<Repositories.ProductServiceDbContext>();

        // repos
        builder.Services.AddScoped<Repositories.LoanRepository>();

        builder.Services.AddHttpContextAccessor();
        builder.AddCisCurrentUser();
        builder.Services.AddCodebookService(true);
            
        return builder;
    }
}
