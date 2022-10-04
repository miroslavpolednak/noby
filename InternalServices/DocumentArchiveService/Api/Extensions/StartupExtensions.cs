using CIS.Infrastructure.StartupExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CIS.InternalServices.DocumentArchiveService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRipService(this WebApplicationBuilder builder)
    {
        // disable default model state validations
        builder.Services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();

        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(Program))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // json
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        });

        // MVC
        builder.Services.AddControllers();

        return builder;
    }

    internal class NullObjectModelValidator : IObjectModelValidator
    {
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public void Validate(ActionContext actionContext,
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
            ValidationStateDictionary validationState, string prefix, object model)
        {

        }
    }
}