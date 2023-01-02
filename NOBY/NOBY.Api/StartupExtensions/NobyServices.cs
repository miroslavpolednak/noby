using ExternalServices.AddressWhisperer;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using NOBY.Infrastructure.Security;
using ExternalServices.AddressWhisperer.V1;

namespace NOBY.Api.StartupExtensions;

internal static class NobyServices
{
    public static WebApplicationBuilder AddNobyServices(this WebApplicationBuilder builder)
    {
        // mediatr
        builder.Services.AddMediatR(typeof(IApiAssembly).Assembly);
        
        // user accessor
        builder.Services.AddTransient<CIS.Core.Security.ICurrentUserAccessor, NobyCurrentUserAccessor>();

        // controllers and validation
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CIS.Infrastructure.WebApi.JsonConverterForNullableDateTime());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;

            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<IApiAssembly>(includeInternalTypes: true);
                fv.DisableDataAnnotationsValidation = true;
            });

        // ext services
        builder.AddExternalService<IAddressWhispererClient>();

        return builder;
    }
}
