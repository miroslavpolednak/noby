using ExternalServices.Rip;
using FluentValidation.AspNetCore;

namespace FOMS.Api.StartupExtensions;

internal static class FomsServices
{
    public static WebApplicationBuilder AddFomsServices(this WebApplicationBuilder builder, Infrastructure.Configuration.AppConfiguration appConfiguration)
    {
        // mediatr
        builder.Services.AddMediatR(typeof(IApiAssembly).Assembly);
        
        // user accessor
        builder.Services.AddTransient<CIS.Core.Security.ICurrentUserAccessor, Infrastructure.Security.FomsCurrentUserAccessor>();

        // controllers and validation
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CIS.Infrastructure.WebApi.JsonConverterForNullableDateTime());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
            })
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
