using FluentValidation;
using ExternalServices;

namespace DomainServices.DocumentService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddDocumentService(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.CisMediatR.GrpcValidationBehavior<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // ESignatures
        builder.AddExternalService<ESignatures.IESignaturesClient>();

        //// dbcontext
        //string connectionString = builder.Configuration.GetConnectionString("default");
        //builder.Services.AddDbContext<Repositories.DocumentServiceDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true), ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        return builder;
    }
}