using CIS.Infrastructure.StartupExtensions;
using ProtoBuf.Grpc.Server;
using DomainServices.CodebookService.Api;
using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.Telemetry;
using Microsoft.OpenApi.Models;
using CIS.Infrastructure.Caching;
using CIS.Infrastructure.Security;
using CIS.InternalServices;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc"));
var endpointsType = typeof(DomainServices.CodebookService.Endpoints.IEndpointsAssembly);
var assembly = endpointsType.Assembly;

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc 
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

#region register builder.Services
// globalni nastaveni prostredi
builder
    .AddCisEnvironmentConfiguration()
    .AddCisCoreFeatures();
builder.Services.AddAttributedServices(typeof(Program), endpointsType);
builder.Services.AddCisDistributedCache();

// logging 
builder
    .AddCisLogging()
    .AddCisTracing();

// add mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

// health checks
builder.AddCisHealthChecks();

// add general Dapper repository
builder.Services
    .AddDapper(builder.Configuration.GetConnectionString("default"))
    .AddDapper<DomainServices.CodebookService.Endpoints.IXxdDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxd"))
    .AddDapper<DomainServices.CodebookService.Endpoints.IKonsdbDapperConnectionProvider>(builder.Configuration.GetConnectionString("konsDb"));

// authentication
builder.AddCisServiceAuthentication();

// current project related
builder
    .AddCodebookService()
    .AddCodebookServiceEndpointsStartup(assembly);

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Codebook Service API", Version = "v1" });

    // všechny parametry budou camel case
    x.DescribeAllParametersInCamelCase();

    x.CustomSchemaIds(type => type.ToString());
});

// add grpc reflection
builder.Services.AddCodeFirstGrpcReflection();
#endregion register builder.Services

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseServiceDiscovery();

app
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Codebook Service API");
    });

app.UseRouting();

// auth
app.UseAuthentication();
app.UseAuthorization();
app.UseCisServiceUserContext();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.CodebookService.Api.Services.CodebookService>();

    endpoints.MapCodeFirstGrpcReflectionService();

    endpoints.MapCodebookJsonApi();
});

// print gRPC PROTO file
//CIS.Infrastructure.gRPC.GrpcHelpers.CreateProtoFileFromContract<Contracts.ICodebookService("d:\\Visual Studio Projects\\MPSS-FOMS\\DomainServices\\CodebookService\\Contracts\\protos\\CodebookService.proto");

try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}