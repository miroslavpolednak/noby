using ExternalServices.ESignatures;
using CIS.Infrastructure.StartupExtensions;
using FluentValidation;

namespace DomainServices.DocumentService.Api;

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

    public static WebApplicationBuilder AddDocumentService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehavior<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // ESignatures
        builder.Services.AddExternalServiceESignatures(appConfiguration.ESignatures);

        //// dbcontext
        //string connectionString = builder.Configuration.GetConnectionString("default");
        //builder.Services.AddDbContext<Repositories.DocumentServiceDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        return builder;
    }
}