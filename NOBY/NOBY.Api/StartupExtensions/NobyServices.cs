using NOBY.Infrastructure.ErrorHandling.Internals;
using CIS.Infrastructure.StartupExtensions;
using MPSS.Security.Noby;
using ExternalServices;

namespace NOBY.Api.StartupExtensions;

internal static class NobyServices
{
    public static WebApplicationBuilder AddNobyServices(this WebApplicationBuilder builder)
    {
        var assemblyType = typeof(IApiAssembly);

        // memory cache
        builder.Services.AddLazyCache();

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

        // dbcontext
        builder.AddEntityFramework<Database.FeApiDbContext>();

        // add distributed cache
        builder.AddCisDistributedCache();

        // ext services
        builder.AddExternalService<ExternalServices.Crem.V1.ICremClient>();
        builder.AddExternalService<ExternalServices.AddressWhisperer.V1.IAddressWhispererClient>(CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.DefaultAppCompOriginatorValue, CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.DefaultAppCompOriginatorValue);

        // pridat mpss cookie
        builder.AddMpssSecurityCookie();

        return builder;
    }
}
