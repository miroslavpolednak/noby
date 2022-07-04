using CIS.Infrastructure.StartupExtensions;
using DomainServices.RiskIntegrationService.Api.Clients;

namespace DomainServices.RiskIntegrationService.Api;

internal static class StartupExtensions
{
    public static WebApplicationBuilder AddRipService(this WebApplicationBuilder builder)
    {
        // add general Dapper repository
        /*builder.Services
            .AddDapper<Shared.IXxvDapperConnectionProvider>(builder.Configuration.GetConnectionString("xxv"));*/

        builder.Services
            .AddMediatR(typeof(Program).Assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CIS.Infrastructure.gRPC.Validation.GrpcValidationBehaviour<,>));

        // json
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
        });

        // MVC
        builder.Services.AddControllers();

        // register c4m clients
        builder.AddRiskBusinessCase();
        builder.AddCreditWorthiness();

        return builder;
    }
}
