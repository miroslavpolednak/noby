﻿using NOBY.Infrastructure.ErrorHandling.Internals;
using CIS.Infrastructure.StartupExtensions;
using MPSS.Security.Noby;
using ExternalServices;
using CIS.Infrastructure.WebApi;

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
            .AddControllers(x =>
            {
                x.Filters.Add(new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None });
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // disable default asp model validation - tohle chteji vypnout
                //options.SuppressModelStateInvalidFilter = true;
                //options.SuppressMapClientErrors = true;

                // misto toho budeme vracet model state chyby
                options.InvalidModelStateResponseFactory = context =>
                {
                    List<ApiErrorItem> errors = context
                        .ModelState
                        .Select(t => new ApiErrorItem
                        {
                            ErrorCode = 90100,
                            Severity = ApiErrorItemServerity.Error,
                            Message = "Nastala neočekávaná chyba, opakujte akci později prosím.",
                            Description = "Chybí povinné parametry.",
                            Reason = new ApiErrorItem.ErrorReason
                            { 
                                ReasonType = "ModelState validation",
                                ReasonDescription = t.Value?.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "" 
                            } 
                        })
                        .ToList();

                    return new BadRequestObjectResult(errors);
                };
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonConverterForNullableDateTime());
                // FE nechce datetime z ms a timezone
                options.JsonSerializerOptions.Converters.Add(new JsonConverterForZonelessDateTime());
                
                // esacping problemovych znaku na vystupu
                //options.JsonSerializerOptions.Converters.Add(new CIS.Infrastructure.WebApi.JsonConverterForStringEncoding());
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

