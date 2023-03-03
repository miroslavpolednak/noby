using ExternalServices.AddressWhisperer;
using System.Text.Json.Serialization;
using NOBY.Infrastructure.Security;
using ExternalServices.AddressWhisperer.V1;

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
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.ErrorHandling.NobyValidationBehavior<,>));

        // add validators
        builder.Services.Scan(selector => selector
            .FromAssembliesOf(assemblyType)
            .AddClasses(x => x.AssignableTo(typeof(FluentValidation.IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // controllers and validation
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CIS.Infrastructure.WebApi.JsonConverterForNullableDateTime());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;

            });

        // add sql distributed cache
        //TODO replace with Redis?
        builder.Services.AddDistributedSqlServerCache(options =>
        {
            options.SchemaName = "dbo";
            options.TableName = "AppCache";
            options.ConnectionString = builder.Configuration.GetConnectionString("distributedCache");
        });

        // ext services
        builder.AddExternalService<IAddressWhispererClient>();

        return builder;
    }
}
