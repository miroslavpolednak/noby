using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.Infrastructure.Telemetry;
using DomainServices.RiskIntegrationService.Api;
using CIS.Infrastructure.Security;
using ProtoBuf.Grpc.Server;
using CIS.InternalServices;
using DomainServices;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc", StringComparison.OrdinalIgnoreCase));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

var log = builder.CreateStartupLogger();

try
{
    AppConfiguration appConfiguration = new();
    builder.Configuration.GetSection(CIS.Core.CisGlobalConstants.DefaultAppConfigurationSectionName).Bind(appConfiguration);

    #region register builder
    // strongly-typed konfigurace aplikace
    builder.Services.AddSingleton(appConfiguration);

    // globalni nastaveni prostredi
    builder
        .AddCisCoreFeatures(true, true)
        .AddCisEnvironmentConfiguration();

    // logging 
    builder
        .AddCisLogging()
        .AddCisLoggingPayloadBehavior()
        .AddCisTracing()
        .AddCisDistributedCache();

    // health checks
    builder.Services.AddAttributedServices(typeof(Program));

    // authentication
    builder.AddCisServiceAuthentication().AddCisServiceUserContext();

    // add this service
    builder.AddRipService();

    // add BE services
    builder.Services
        .AddUserService()
        .AddCodebookService()
        .AddCisServiceDiscovery();

    // swagger
    builder.AddRipSwagger();

    // add grpc
    builder.AddRipGrpc();
    builder.Services.AddHealthChecks();
    #endregion register builder

    // kestrel configuration
    builder.UseKestrelWithCustomConfiguration();

    // BUILD APP
    if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
    var app = builder.Build();
    log.ApplicationBuilt();

    app.UseServiceDiscovery();

    // zachytavat vyjimky pri WebApi volani a transformovat je do 400 bad request
    app.UseGrpc2WebApiException();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCisServiceUserContext();

    app.MapCodeFirstGrpcHealthChecks();
    app.MapHealthChecks(CIS.Core.CisGlobalConstants.CisHealthCheckEndpointUrl, new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
    });

    app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.CustomerExposure.V2.CustomersExposureService>();
    app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.RiskBusinessCaseService>();
    app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.CreditWorthinessService>();
    app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.LoanApplicationService>();
    app.MapGrpcService<DomainServices.RiskIntegrationService.Api.Endpoints.TestServiceGrpc>();

    app.MapCodeFirstGrpcReflectionService();

    app.MapControllers();

    // swagger
    app.UseRipSwagger();

    // print gRPC PROTO file
    //var schemaGenerator = new ProtoBuf.Grpc.Reflection.SchemaGenerator();
    //var proto1 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2.ICreditWorthinessService>();
    //File.WriteAllText("d:\\Visual Studio Projects\\MPSS-FOMS\\ProtoExport\\RiskIntegrationService\\CreditWorthinessService.proto", proto1);
    //var proto2 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.CustomerExposure.V2.ICustomerExposureService>();
    //File.WriteAllText("d:\\Visual Studio Projects\\MPSS-FOMS\\ProtoExport\\RiskIntegrationService\\CustomerExposureService.proto", proto2);
    //var proto3 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.ILoanApplicationService>();
    //File.WriteAllText("d:\\Visual Studio Projects\\MPSS-FOMS\\ProtoExport\\RiskIntegrationService\\LoanApplicationService.proto", proto3);
    //var proto6 = schemaGenerator.GetSchema<DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2.IRiskBusinessCaseService>();
    //File.WriteAllText("d:\\Visual Studio Projects\\MPSS-FOMS\\ProtoExport\\RiskIntegrationService\\RiskBusinessCaseService.proto", proto6);

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

#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}