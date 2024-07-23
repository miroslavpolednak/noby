using NOBY.Infrastructure.ErrorHandling.Internals;
using CIS.Infrastructure.StartupExtensions;
using MPSS.Security.Noby;
using ExternalServices;
using System.Text.Json;

namespace NOBY.Api.StartupExtensions;

internal static class NobyServices
{
    public static WebApplicationBuilder AddNobyServices(this WebApplicationBuilder builder, Infrastructure.Configuration.AppConfiguration appConfiguration)
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

        if (appConfiguration.LogRequestContractDifferences)
        {
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(NobyAdditionalRequestPropertiesLoggerBehavior<,>));
        }

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
                //TODO ODSTRANIT az FE bude reflektovat rozdil mezi datetime a date !!!!!!!!!!!!!!!!!!!!!!!
                options.JsonSerializerOptions.Converters.Add(new TempJsonConverterForDateOnly());
                options.JsonSerializerOptions.Converters.Add(new TempJsonConverterForNullableDateOnly());
            });

        // dbcontext
        builder.AddEntityFramework<Database.FeApiDbContext>();

        // add distributed cache
        builder.AddCisDistributedCache();

        // ext services
        builder.AddExternalService<ExternalServices.Crem.V1.ICremClient>();
        builder.AddExternalService<ExternalServices.AddressWhisperer.V1.IAddressWhispererClient>(CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.DefaultAppCompOriginatorValue, CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.KbHeadersHttpHandler.DefaultAppCompOriginatorValue);
        builder.AddExternalService<ExternalServices.RuianAddress.V1.IRuianAddressClient>();
        builder.AddExternalService<ExternalServices.SbWebApi.V1.ISbWebApiClient>();
        builder.AddExternalService<ExternalServices.Party.V1.IPartyClient>();

        // pridat mpss cookie
        builder.AddMpssSecurityCookie();

        return builder;
    }
}

public sealed class TempJsonConverterForDateOnly
    : System.Text.Json.Serialization.JsonConverter<DateOnly>
{
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return DateOnly.MaxValue;
        }
        else
        {
            return reader.TryGetDateTime(out DateTime d) ? DateOnly.FromDateTime(d) : DateOnly.MaxValue;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        if (value == DateOnly.MinValue)
        {
            writer.WriteStringValue("");
        }
        else
        {
            writer.WriteStringValue($"{value:yyyy-MM-dd}");
        }
    }
}

public sealed class TempJsonConverterForNullableDateOnly
    : System.Text.Json.Serialization.JsonConverter<DateOnly?>
{
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else
        {
            return reader.TryGetDateTime(out DateTime d) ? DateOnly.FromDateTime(d) : null;
        }
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (!value.HasValue)
        {
            writer.WriteStringValue("");
        }
        else
        {
            writer.WriteStringValue($"{value.Value:yyyy-MM-dd}");
        }
    }
}