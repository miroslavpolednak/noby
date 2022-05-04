using ExternalServices.Rip;
using FluentValidation.AspNetCore;

namespace FOMS.Api.StartupExtensions;

internal static class FomsServices
{
    public static WebApplicationBuilder AddFomsServices(this WebApplicationBuilder builder, Infrastructure.Configuration.AppConfiguration appConfiguration)
    {
        // mediatr
        builder.Services.AddMediatR(typeof(IApiAssembly).Assembly);
        
        // json
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        });

        // user accessor
        builder.Services.AddTransient<CIS.Core.Security.ICurrentUserAccessor, Infrastructure.Security.FomsCurrentUserAccessor>();

        // controllers and validation
        builder.Services
            .AddControllers()
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<IApiAssembly>(includeInternalTypes: true);
                fv.DisableDataAnnotationsValidation = true;
            });

        // RIP
        builder.Services.AddExternalServiceRip(appConfiguration.Rip);

        return builder;
    }
}
