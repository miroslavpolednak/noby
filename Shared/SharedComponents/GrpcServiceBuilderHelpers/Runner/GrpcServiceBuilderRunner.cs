using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using SharedAudit;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Security;
using CIS.Infrastructure.CisMediatR;
using CIS.InternalServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using DomainServices;
using Microsoft.OpenApi.Models;

namespace SharedComponents.GrpcServiceBuilderHelpers;

internal sealed class GrpcServiceBuilderRunner<TConfiguration>
    where TConfiguration : class
{
    public void Run()
    {
        // provest validaci nastaveni
        validation();

        var log = _settings.Builder.CreateStartupLogger();

        try
        {
            _settings.Builder.AddCisCoreFeatures();
            _settings.Builder.Services.AddAttributedServices(_settings.MainAssembly);

            _settings.Builder
                // logging
                .AddCisLogging()
                .AddCisLoggingPayloadBehavior()
                .AddCisTracing()
                // authentication
                .AddCisServiceAuthentication();

            // end user context
            if (!_settings.SkipServiceUserContext)
            {
                _settings.Builder
                    .AddCisServiceUserContext()
                    .AddCisAudit();
            }

            // rollback
            if (_settings.AddRollbackCapability)
            {
                _settings.Builder.Services.AddCisMediatrRollbackCapability();
            }

            // distributed cache
            if (_settings.AddDistributedCache)
            {
                _settings.Builder.AddCisDistributedCache();
            }

            // add service discovery for all consuments
            _settings.Builder.Services.AddCisServiceDiscovery();

            // add domain and internal services
            addRequiredServices();

            // add grpc infrastructure
            var grpcBuilder = _settings.Builder.Services
                .AddCisGrpcInfrastructure(_settings.MainAssembly, _settings.ErrorCodeMapperMessages)
                .AddGrpc(options =>
                {
                    options.Interceptors.Add<GenericServerExceptionInterceptor>();
                    options.MaxReceiveMessageSize = 4 * 1024 * 1024; // 4 MB
                    options.MaxSendMessageSize = 4 * 1024 * 1024; // 4 MB

                    // alter options if needed
                    _settings.AddGrpcServiceOptions?.Invoke(options);
                });

            // grpc reflection
            if (!_settings.EnvironmentConfiguration.DisableContractDescriptionPropagation)
            {
                _settings.Builder.Services.AddGrpcReflection();
            }

            // json transcoding
            if (_settings.EnableJsonTranscoding)
            {
                grpcBuilder.AddJsonTranscoding(o =>
                {
                    o.JsonSettings.WriteEnumsAsIntegers = true;
                });

                if (!_settings.EnvironmentConfiguration.DisableContractDescriptionPropagation)
                {
                    addSwagger();
                }
            }

            // add HC
            _settings.Builder.AddCisGrpcHealthChecks();

            // kestrel configuration
            _settings.Builder.UseKestrelWithCustomConfiguration();

            // custom services
            if (_settings.AddCustomServicesWithConfiguration is not null && _settings.Configuration is not null)
            {
                _settings.AddCustomServicesWithConfiguration(_settings.Builder, _settings.Configuration);
            }
            else if (_settings.AddCustomServices is not null)
            {
                _settings.AddCustomServices(_settings.Builder);
            }

            // BUILD APP
            if (_settings.RunAsWindowsService)
            {
                _settings.Builder.Host.UseWindowsService(); // run as win svc
            }
            var app = _settings.Builder.Build();
            log.ApplicationBuilt();

            // default
            app.UseServiceDiscovery();
            
            if (_settings.EnableJsonTranscoding)
            {
                app.UseHsts();
            }
            app.UseRouting();

            app.UseAuthentication()
                .UseAuthorization()
                .UseCisServiceUserContext();

            app.MapCisGrpcHealthChecks();

            if (_isGenericRunner)
            {
                if (_settings.UseMiddlewares is not null)
                {
                    _settings.UseMiddlewares!(app);
                }
                
                _settings.MapGrpcServices!(app);
            }
            else
            {
                if (_settings.UseMiddlewaresT is not null)
                {
                    _settings.UseMiddlewaresT!(app, _settings.Configuration!);
                }

                _settings.MapGrpcServicesT!(app, _settings.Configuration!);
            }

            // grpc transcoding swagger / grpc reflection
            if (!_settings.EnvironmentConfiguration.DisableContractDescriptionPropagation)
            {
                app.MapGrpcReflectionService();

                if (_settings.EnableJsonTranscoding)
                {
                    useSwagger(app);
                }
            }
            
            log.ApplicationRun();
            app.Run();
        }
        catch (Exception ex)
        {
            log.CatchedException(ex);
        }
        finally
        {
            LoggingExtensions.CloseAndFlush();
        }
    }

    private void addSwagger()
    {
        _settings.Builder.Services.AddGrpcSwagger();
        _settings.Builder.Services.AddEndpointsApiExplorer();

        _settings.Builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(_settings.TranscodingOptions!.OpenApiVersion, new OpenApiInfo 
            { 
                Title = _settings.TranscodingOptions.OpenApiTitle ?? "", 
                Version = _settings.TranscodingOptions.OpenApiVersion 
            });

            // generate the XML docs that'll drive the swagger docs
            if (_settings.TranscodingOptions.OpenApiXmlCommentsPaths?.Any() ?? false)
            {
                _settings.TranscodingOptions.OpenApiXmlCommentsPaths.ForEach(path =>
                {
                    c.IncludeXmlComments(path);
                    c.IncludeGrpcXmlComments(path, includeControllerXmlComments: true);
                });
            }
            
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Service user login"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        Array.Empty<string>()
                }
            });

            c.SupportNonNullableReferenceTypes();
            c.UseAllOfToExtendReferenceSchemas();
            c.DescribeAllParametersInCamelCase();
            c.UseInlineDefinitionsForEnums();
            c.CustomSchemaIds(type => type.ToString().Replace('+', '_'));
            //c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");

            c.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
        });
    }

    private void useSwagger(WebApplication app)
    {
        if (_settings.EnableJsonTranscoding)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{_settings.TranscodingOptions!.OpenApiVersion}/swagger.json", _settings.TranscodingOptions!.OpenApiEndpointVersion);
                c.DisplayOperationId();
            });
        }
    }

    private void addRequiredServices()
    {
        if (_settings.RequiredServicesBuilder is not null)
        {
            if (_settings.RequiredServicesBuilder.CaseService)
            {
                _settings.Builder.Services.AddCaseService();
            }
            if (_settings.RequiredServicesBuilder.CodebookService)
            {
                _settings.Builder.Services.AddCodebookService();
            }
            if (_settings.RequiredServicesBuilder.CustomerService)
            {
                _settings.Builder.Services.AddCustomerService();
            }
            if (_settings.RequiredServicesBuilder.DocumentArchiveService)
            {
                _settings.Builder.Services.AddDocumentArchiveService();
            }
            if (_settings.RequiredServicesBuilder.DocumentOnSAService)
            {
                _settings.Builder.Services.AddDocumentOnSAService();
            }
            if (_settings.RequiredServicesBuilder.HouseholdService)
            {
                _settings.Builder.Services.AddHouseholdService();
            }
            if (_settings.RequiredServicesBuilder.OfferService)
            {
                _settings.Builder.Services.AddOfferService();
            }
            if (_settings.RequiredServicesBuilder.ProductService)
            {
                _settings.Builder.Services.AddProductService();
            }
            if (_settings.RequiredServicesBuilder.RealEstateValuationService)
            {
                _settings.Builder.Services.AddRealEstateValuationService();
            }
            if (_settings.RequiredServicesBuilder.RiskIntegrationService)
            {
                _settings.Builder.Services.AddRiskIntegrationService();
            }
            if (_settings.RequiredServicesBuilder.SalesArrangementService)
            {
                _settings.Builder.Services.AddSalesArrangementService();
            }
            if (_settings.RequiredServicesBuilder.UserService)
            {
                _settings.Builder.Services.AddUserService();
            }
            if (_settings.RequiredServicesBuilder.DocumentGeneratorService)
            {
                _settings.Builder.Services.AddDocumentGeneratorService();
            }
            if (_settings.RequiredServicesBuilder.DataAggregatorService)
            {
                _settings.Builder.Services.AddDataAggregatorService();
            }
        }
    }

    private void validation()
    {
        if (_settings.MapGrpcServices is null)
        {
            throw new CisConfigurationException(0, "GrpcServiceBuilder: MapGrpcServices action has not been set");
        }
    }

    private readonly bool _isGenericRunner;
    private readonly GrpcServiceBuilderSettings<TConfiguration> _settings;

    internal GrpcServiceBuilderRunner(GrpcServiceBuilderSettings<TConfiguration> settings, bool isGenericRunner)
    {
        _settings = settings;
        _isGenericRunner = isGenericRunner;
    }
}