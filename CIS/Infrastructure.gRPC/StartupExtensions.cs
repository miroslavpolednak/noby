using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.Infrastructure.gRPC;

public static class StartupExtensions
{
    /// <summary>
    /// Zaregistruje do DI:
    /// - MediatR
    /// - FluentValidation through MediatR pipelines
    /// </summary>
    /// <param name="assemblyType">Typ, který je v hlavním projektu - typicky Program.cs</param>
    public static IServiceCollection AddCisGrpcInfrastructure(this IServiceCollection services, Type assemblyType)
    {
        services
            .AddMediatR(assemblyType.Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(Validation.GrpcValidationBehaviour<,>));

        // add validators
        services.Scan(selector => selector
                .FromAssembliesOf(assemblyType)
                .AddClasses(x => x.AssignableTo(typeof(FluentValidation.IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        return services;
    }
}
