using CIS.Infrastructure.gRPC;
using CIS.Infrastructure.StartupExtensions;
using CIS.InternalServices.Storage.Api;
using CIS.Security;

bool runAsWinSvc = args != null && args.Any(t => t.Equals("winsvc"));

//TODO workaround until .NET6 UseWindowsService() will work with WebApplication
var webAppOptions = runAsWinSvc
    ?
    new WebApplicationOptions { Args = args, ContentRootPath = AppContext.BaseDirectory }
    :
    new WebApplicationOptions { Args = args };
var builder = WebApplication.CreateBuilder(webAppOptions);

#region register builder.Services
// globalni nastaveni prostredi
builder.Services.AddCisEnvironmentConfiguration(builder.Configuration);

// logging 
builder.Host.AddCisLogging();

// add mediatr
builder.Services.AddMediatR(typeof(Program).Assembly);

// health checks
builder.Services.AddCisHealthChecks(builder.Configuration);

builder.Services.AddCisCoreFeatures();
builder.Services.AddAttributedServices(typeof(Program));

// add general Dapper repository
builder.Services.AddDapper(builder.Configuration.GetConnectionString("default"));

// authentication
builder.Services.AddCisServiceAuthentication(builder.Configuration);

// add storage services
builder.Services.AddBlobStorage(builder.Configuration);

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.SimpleServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
#endregion register builder.Services

// kestrel configuration
builder.UseKestrelWithCustomConfiguration();

// BUILD APP
if (runAsWinSvc) builder.Host.UseWindowsService(); // run as win svc
var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<CIS.InternalServices.Storage.Api.BlobStorage.Services.BlobService>();
    endpoints.MapGrpcService<CIS.InternalServices.Storage.Api.BlobStorage.Services.BlobTempService>();

    endpoints.MapGrpcReflectionService();
});

app.Run();