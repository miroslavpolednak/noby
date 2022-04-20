using CIS.Infrastructure.StartupExtensions;
using FluentValidation;

namespace DomainServices.RiskIntegrationService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRiskIntegrationService(this WebApplicationBuilder builder, AppConfiguration appConfiguration)
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

        builder.AddCisCurrentUser();
            
        return builder;
    }
}
