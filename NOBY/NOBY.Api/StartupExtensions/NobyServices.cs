using ExternalServices.AddressWhisperer;
using NOBY.Infrastructure.Security;
using ExternalServices.AddressWhisperer.V1;
using NOBY.Infrastructure.ErrorHandling.Internals;
using CIS.Infrastructure.StartupExtensions;
using NOBY.Infrastructure.Services;
using MPSS.Security.Noby;

namespace NOBY.Api.StartupExtensions;

internal static class NobyServices
{
    public static WebApplicationBuilder AddNobyServices(this WebApplicationBuilder builder)
    {
        var assemblyType = typeof(IApiAssembly);

        // user accessor
        builder.Services.AddTransient<CIS.Core.Security.ICurrentUserAccessor, NobyCurrentUserAccessor>();

        // disable default asp model validation
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        builder.Services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assemblyType.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(NobyValidationBehavior<,>));

        // add validators
        builder.Services.Scan(selector => selector
            .FromAssembliesOf(assemblyType)
            .AddClasses(x => x.AssignableTo(typeof(FluentValidation.IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        // controllers and validation
        builder.Services
            .AddControllers(x => x.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None }))
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CIS.Infrastructure.WebApi.JsonConverterForNullableDateTime());
            });

        // flow switches
        builder.Services.AddFlowSwitches(builder.Configuration.GetConnectionString("default")!);

        // add distributed cache
        builder.AddCisDistributedCache();

        // ext services
        builder.AddExternalService<IAddressWhispererClient>(CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.DefaultAppCompOriginatorValue, CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.DefaultAppCompOriginatorValue);

        // pridat mpss cookie
        builder.AddMpssSecurityCookie();

        return builder;
    }
}
