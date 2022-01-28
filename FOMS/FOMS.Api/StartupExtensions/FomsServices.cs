using FluentValidation;

namespace FOMS.Api.StartupExtensions;

internal static class FomsServices
{
    public static WebApplicationBuilder AddFomsServices(this WebApplicationBuilder builder)
    {
        // mediatr
        builder.Services
            .AddMediatR(typeof(IApiAssembly).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(WebApiValidationBehaviour<,>));

        // add validators
        builder.Services.Scan(selector => selector
                .FromAssembliesOf(typeof(IApiAssembly))
                .AddClasses(x => x.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        // json
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        });

        builder.Services.AddHttpContextAccessor();

        // user accessor
        builder.Services.AddTransient<CIS.Core.Security.ICurrentUserAccessor, Infrastructure.Security.FomsCurrentUserAccessor>();
        
        // doc processor factory
        builder.Services.AddTransient<DocumentProcessing.IDocumentProcessorFactory, DocumentProcessing.DocumentProcessorFactory>();

        return builder;
    }
}
